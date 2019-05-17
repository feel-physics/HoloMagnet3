// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#define feel_physics

using HoloToolkit.Unity.UX;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace HoloToolkit.Unity.InputModule.Utilities.Interactions
{
    /// <summary>
    /// This script allows for an object to be movable, scalable, and rotatable with one or two hands. 
    /// You may also configure the script on only enable certain manipulations. The script works with 
    /// both HoloLens' gesture input and immersive headset's motion controller input.
    /// See Assets/HoloToolkit-Examples/Input/Readme/README_TwoHandManipulationTest.md
    /// for instructions on how to use the script.
    /// </summary>
    public class BarMagnetTwoHandManipulatable : MonoBehaviour, IInputHandler, ISourceStateHandler
    {
        [SerializeField]
        [Tooltip("Transform that will be dragged. Defaults to the object of the component.")]
        private Transform hostTransform = null;

        public Transform HostTransform
        {
            get { return hostTransform; }
            set { hostTransform = value; }
        }

        [SerializeField]
        [Tooltip("To visualize the object bounding box, drop the HoloToolkit/UX/Prefabs/BoundingBoxes/BoundingBoxBasic.prefab here. This is optional.")]
        private BoundingBox boundingBoxPrefab = null;

        /// <summary>
        /// Reference to the Prefab from which clone is instantiated.
        /// </summary>
        public BoundingBox BoundingBoxPrefab
        {
            set { boundingBoxPrefab = value; }
            get { return boundingBoxPrefab; }
        }

        [SerializeField]
        [Tooltip("What manipulation will two hands perform?")]
        private ManipulationMode manipulationMode = ManipulationMode.Scale;

        public ManipulationMode ManipulationMode
        {
            get { return manipulationMode; }
            set { manipulationMode = value; }
        }

        [SerializeField]
        [Tooltip("Constrain rotation along an axis")]
        private AxisConstraint rotationConstraint = AxisConstraint.None;

        public AxisConstraint RotationConstraint
        {
            get { return rotationConstraint; }
            set
            {
                rotationConstraint = value;
                rotateLogic = new TwoHandRotateLogic(rotationConstraint);
            }
        }

        [SerializeField]
        [Tooltip("If true, grabbing the object with one hand will initiate movement.")]
        private bool enableOneHandMovement = true;

        public bool EnableEnableOneHandedMovement
        {
            get { return enableOneHandMovement; }
            set { enableOneHandMovement = value; }
        }

#if feel_physics
        [SerializeField]
        private bool Is2D = false;
#endif

        // Private fields that store transform information.
        #region Transform Info

        private BoundingBox boundingBoxInstance;
        private ManipulationMode currentState;
        private TwoHandMoveLogic moveLogic;
        private TwoHandScaleLogic scaleLogic;
        private TwoHandRotateLogic rotateLogic;

        #endregion Transform Info

        /// <summary>
        /// Maps input id -> position of hand.
        /// </summary>
        private readonly Dictionary<uint, Vector3> handsPressedLocationsMap = new Dictionary<uint, Vector3>();

        /// <summary>
        /// Maps input id -> input source. Then obtain position of input source using currentInputSource.TryGetGripPosition(currentInputSourceId, out inputPosition);
        /// </summary>
        private readonly Dictionary<uint, IInputSource> handsPressedInputSourceMap = new Dictionary<uint, IInputSource>();

        /// <summary>
        /// Property that turns on and off the Visibility of the BoundingBox cloned from the BoundingBoxPrefab reference.
        /// </summary>
        private bool ShowBoundingBox
        {
            set
            {
                if (boundingBoxPrefab != null)
                {
                    if (boundingBoxInstance == null)
                    {
                        // Instantiate Bounding Box from the Prefab
                        boundingBoxInstance = Instantiate(boundingBoxPrefab) as BoundingBox;
                    }

                    if (value)
                    {
                        boundingBoxInstance.Target = HostTransform.gameObject;
                        boundingBoxInstance.gameObject.SetActive(true);
                    }
                    else
                    {
                        boundingBoxInstance.Target = null;
                        boundingBoxInstance.gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Change the manipulation mode.
        /// </summary>
        [System.Obsolete("Use ManipulationMode.")]
        public void SetManipulationMode(ManipulationMode mode)
        {
            manipulationMode = mode;
        }

        private void Awake()
        {
            moveLogic = new TwoHandMoveLogic();
            rotateLogic = new TwoHandRotateLogic(rotationConstraint);
            scaleLogic = new TwoHandScaleLogic();
        }

        private void Start()
        {
            if (hostTransform == null)
            {
                hostTransform = transform;
            }

#if feel_physics
            // 2次元のシーンではz=2で固定（初期化）
            if (MySceneManager.Instance.MyScene ==
                MySceneManager.MySceneEnum.Compasses_2D)
            {
                Is2D = true;
            }
#endif

        }

        private void Update()
        {
            // Update positions of all hands
            foreach (var key in handsPressedInputSourceMap.Keys)
            {
                var inputSource = handsPressedInputSourceMap[key];
                Vector3 inputPosition;
                if (inputSource.TryGetGripPosition(key, out inputPosition))
                {
                    handsPressedLocationsMap[key] = inputPosition;
                }
            }

            if (currentState != ManipulationMode.None)
            {
                UpdateStateMachine();
            }

#if feel_physics
            // 2次元のシーンではz=2で固定
            if (Is2D)
            {
                /*
                 * 参考にさせていただきました
                 * C#でpositionのxとかyに値を突っ込む - テラシュールブログ
                 * http://tsubakit1.hateblo.jp/entry/20131015/1381836858
                 */
                Vector3 pos = transform.position;
                Vector3 newpos = new Vector3(pos.x, pos.y, 2);
                transform.position = newpos;
            }
#endif
        }

        private Vector3 GetInputPosition(InputEventData eventData)
        {
            Vector3 result;
            eventData.InputSource.TryGetGripPosition(eventData.SourceId, out result);
            return result;
        }

        private void RemoveSourceIdFromHandMap(uint sourceId)
        {
            if (handsPressedLocationsMap.ContainsKey(sourceId))
            {
                handsPressedLocationsMap.Remove(sourceId);
            }

            if (handsPressedInputSourceMap.ContainsKey(sourceId))
            {
                handsPressedInputSourceMap.Remove(sourceId);
            }
        }

        /// <summary>
        /// Event Handler receives input from inputSource.
        /// </summary>
        public void OnInputDown(InputEventData eventData)
        {
            // Add to hand map
            handsPressedLocationsMap[eventData.SourceId] = GetInputPosition(eventData);
            handsPressedInputSourceMap[eventData.SourceId] = eventData.InputSource;
            UpdateStateMachine();
            eventData.Use();
        }

        /// <summary>
        /// Event Handler receives input from inputSource.
        /// </summary>
        public void OnInputUp(InputEventData eventData)
        {
            RemoveSourceIdFromHandMap(eventData.SourceId);
            UpdateStateMachine();
            eventData.Use();
        }

        /// <summary>
        /// OnSourceDetected Event Handler.
        /// </summary>
        public void OnSourceDetected(SourceStateEventData eventData) { }

        /// <summary>
        /// OnSourceLost Event Handler.
        /// </summary>
        public void OnSourceLost(SourceStateEventData eventData)
        {
            RemoveSourceIdFromHandMap(eventData.SourceId);
            UpdateStateMachine();
            eventData.Use();
        }

        /// <summary>
        /// Updates the state machine based on the current state and anything that might have changed with the hands.
        /// </summary>
        private void UpdateStateMachine()
        {
            var handsPressedCount = handsPressedLocationsMap.Count;
            ManipulationMode newState = currentState;

            switch (currentState)
            {
                case ManipulationMode.None:
                case ManipulationMode.Move:
                    if (handsPressedCount == 0)
                    {
                        newState = ManipulationMode.None;
                    }
                    else if (handsPressedCount == 1)
                    {
                        newState = enableOneHandMovement ? ManipulationMode.Move : ManipulationMode.None;
                    }
                    else if (handsPressedCount > 1)
                    {
                        newState = manipulationMode;
                    }
                    break;
                case ManipulationMode.Scale:
                case ManipulationMode.Rotate:
                case ManipulationMode.MoveAndScale:
                case ManipulationMode.MoveAndRotate:
                case ManipulationMode.RotateAndScale:
                case ManipulationMode.MoveScaleAndRotate:
                    if (handsPressedCount == 0)
                    {
                        newState = ManipulationMode.None;
                    }
                    else if (handsPressedCount == 1)
                    {
                        newState = enableOneHandMovement ? ManipulationMode.Move : ManipulationMode.None;
                    }
                    break;
            }

            InvokeStateUpdateFunctions(currentState, newState);
            currentState = newState;
        }

        private void InvokeStateUpdateFunctions(ManipulationMode oldState, ManipulationMode newState)
        {
            if (newState != oldState)
            {
                switch (newState)
                {
                    case ManipulationMode.None:
                        OnManipulationEnded();
                        break;
                    case ManipulationMode.Move:
                        OnOneHandMoveStarted();
                        break;
                    case ManipulationMode.Scale:
                    case ManipulationMode.Rotate:
                    case ManipulationMode.MoveAndScale:
                    case ManipulationMode.MoveAndRotate:
                    case ManipulationMode.RotateAndScale:
                    case ManipulationMode.MoveScaleAndRotate:
                        OnTwoHandManipulationStarted(newState);
                        break;
                }

                switch (oldState)
                {
                    case ManipulationMode.None:
                        OnManipulationStarted();
                        break;
                    case ManipulationMode.Move:
                        break;
                    case ManipulationMode.Scale:
                    case ManipulationMode.Rotate:
                    case ManipulationMode.MoveAndScale:
                    case ManipulationMode.MoveAndRotate:
                    case ManipulationMode.RotateAndScale:
                    case ManipulationMode.MoveScaleAndRotate:
                        OnTwoHandManipulationEnded();
                        break;
                }
            }
            else
            {
                switch (newState)
                {
                    case ManipulationMode.None:
                        break;
                    case ManipulationMode.Move:
                        OnOneHandMoveUpdated();
                        break;
                    case ManipulationMode.Scale:
                    case ManipulationMode.Rotate:
                    case ManipulationMode.MoveAndScale:
                    case ManipulationMode.MoveAndRotate:
                    case ManipulationMode.RotateAndScale:
                    case ManipulationMode.MoveScaleAndRotate:
                        OnTwoHandManipulationUpdated();
                        break;
                }
            }
        }

        private void OnTwoHandManipulationUpdated()
        {
#if UNITY_2017_2_OR_NEWER
            var targetRotation = hostTransform.rotation;
            var targetPosition = hostTransform.position;
            var targetScale = hostTransform.localScale;

            if ((currentState & ManipulationMode.Move) > 0)
            {
                targetPosition = moveLogic.Update(GetHandsCentroid(), targetPosition);
            }

            if ((currentState & ManipulationMode.Rotate) > 0)
            {
                targetRotation = rotateLogic.Update(handsPressedLocationsMap, hostTransform, targetRotation);
            }

            if ((currentState & ManipulationMode.Scale) > 0)
            {
                targetScale = scaleLogic.UpdateMap(handsPressedLocationsMap);
            }

            hostTransform.position = targetPosition;
            hostTransform.rotation = targetRotation;
            hostTransform.localScale = targetScale;
#endif // UNITY_2017_2_OR_NEWER
        }

        private void OnOneHandMoveUpdated()
        {
            var targetPosition = moveLogic.Update(handsPressedLocationsMap.Values.First(), hostTransform.position);

            hostTransform.position = targetPosition;
        }

        private void OnTwoHandManipulationEnded()
        {
#if UNITY_2017_2_OR_NEWER
            // This implementation currently does nothing
#endif // UNITY_2017_2_OR_NEWER
        }

        private Vector3 GetHandsCentroid()
        {
            Vector3 result = handsPressedLocationsMap.Values.Aggregate(Vector3.zero, (current, state) => current + state);
            return result / handsPressedLocationsMap.Count;
        }

        private void OnTwoHandManipulationStarted(ManipulationMode newState)
        {
#if UNITY_2017_2_OR_NEWER
            if ((newState & ManipulationMode.Rotate) > 0)
            {
                rotateLogic.Setup(handsPressedLocationsMap, hostTransform);
            }

            if ((newState & ManipulationMode.Move) > 0)
            {
                moveLogic.Setup(GetHandsCentroid(), hostTransform);
            }

            if ((newState & ManipulationMode.Scale) > 0)
            {
                scaleLogic.Setup(handsPressedLocationsMap, hostTransform);
            }
#endif // UNITY_2017_2_OR_NEWER
        }

        private void OnOneHandMoveStarted()
        {
            Assert.IsTrue(handsPressedLocationsMap.Count == 1);

            moveLogic.Setup(handsPressedLocationsMap.Values.First(), hostTransform);
        }

        private void OnManipulationStarted()
        {
#if feel_physics
            MultiTapHandler.Instance.OnManipulationStarted();

            //手のモデルを表示する
            if (BarMagnetModel.Instance.handReference != null)
                BarMagnetModel.Instance.handReference.SetActive(true);

            // 3次元のシーンであれば自動移動を止める
            if (MySceneManager.Instance.MyScene == MySceneManager.MySceneEnum.Compasses_3D &&
                BarMagnetAutoMover.Instance.IsMoving)
            {
                BarMagnetAutoMover.Instance.IsMoving = false;
            }
#else
            InputManager.Instance.PushModalInputHandler(gameObject);
#endif

            // Show Bounding Box visual on manipulation interaction
            ShowBoundingBox = true;
        }

        private void OnManipulationEnded()
        {
            InputManager.Instance.PopModalInputHandler();

#if feel_physics
            MultiTapHandler.Instance.OnManipulationEnded();

            //手のモデルを非表示にする
            if (BarMagnetModel.Instance.handReference != null)
                BarMagnetModel.Instance.handReference.SetActive(false);
#endif

            // Hide Bounding Box visual on release
            ShowBoundingBox = false;
        }
    }
}

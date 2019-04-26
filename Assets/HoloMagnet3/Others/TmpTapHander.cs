using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TmpTapHander : MonoBehaviour, IInputClickHandler
{
    public void Start()
    {
        // FallBackEventHandlerにする
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    /*
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // ローカルApplicationManagerの状態を1つ進める
        ApplicationStateModel.Instance.Shift();
    }
    */

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Tapped");
        switch (MySceneModelAndLoader.Instance.MyScene)
        {
            case MySceneModelAndLoader.MySceneEnum.Introduction:
                MySceneModelAndLoader.Instance.MyScene = MySceneModelAndLoader.MySceneEnum.Compass_One;
                break;
            case MySceneModelAndLoader.MySceneEnum.Compass_One:
                MySceneModelAndLoader.Instance.MyScene = MySceneModelAndLoader.MySceneEnum.Compasses_2D;
                break;
            case MySceneModelAndLoader.MySceneEnum.Compasses_2D:
                MySceneModelAndLoader.Instance.MyScene = MySceneModelAndLoader.MySceneEnum.Compasses_3D;
                break;
            case MySceneModelAndLoader.MySceneEnum.Compasses_3D:
                break;
            default:
                break;
        };
    }
}

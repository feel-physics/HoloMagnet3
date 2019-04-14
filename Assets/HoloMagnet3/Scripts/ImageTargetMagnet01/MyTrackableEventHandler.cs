/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

#define CHANGE_UEDA  // カーソルの色が変わるように改造した
using FeelPhysics.HoloMagnet36;
using HoloToolkit.Sharing;
using UnityEngine;
using Vuforia;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class MyTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{

#if CHANGE_UEDA
    public GameObject Cursor;
    public Material BlueMaterial;
    public Material RedMaterial;
    public GameObject ApplicationManager;
#endif

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

#if CHANGE_UEDA
        Cursor.GetComponent<Renderer>().material = BlueMaterial;
        SetIsTrakingArMarkerOfApplicationManager(true);
#endif
    }

    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;

#if CHANGE_UEDA
        Cursor.GetComponent<Renderer>().material = RedMaterial;
        SetIsTrakingArMarkerOfApplicationManager(false);
#endif
    }

    #endregion // PROTECTED_METHODS

#if CHANGE_UEDA
    private void SetIsTrakingArMarkerOfApplicationManager(bool isTracking)
    {
        // 誰かがトラッキングしているあいだは他のHoloLensはトラッキングできなくしている
        // 必要かどうか不明
        // このスクリプトでは、カーソルの色の変化も行っている。同じスクリプトでいろいろなことをやるのは良くないが…
        /*
        if (SharingStage.Instance.IsConnected)
        {
            Debug.Log("ApplicationManager.GetComponent<ApplicationParams>().IsTrackingArMarker = isTracking;");
            ApplicationManager.GetComponent<ApplicationParams>().IsTrackingArMarker = isTracking;

            Debug.Log("var sam = GameObject.FindWithTag(SpawnedApplicationManager);");
            var sam = GameObject.FindWithTag("SpawnedApplicationManager");

            Debug.Log("var ssam = (SyncSpawnedApplicationManager)sam." + 
                "GetComponent<DefaultSyncModelAccessor>().SyncModel;");
            var ssam = (SyncSpawnedApplicationManager)sam.
                GetComponent<DefaultSyncModelAccessor>().SyncModel;
            ssam.isTrackingArMarker.Value = isTracking;
        }
        */
    }
}
#endif
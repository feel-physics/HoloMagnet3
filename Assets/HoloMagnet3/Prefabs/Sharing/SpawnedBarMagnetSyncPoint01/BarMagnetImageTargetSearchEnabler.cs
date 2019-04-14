using UnityEngine;
using Vuforia;
using FeelPhysics.HoloMagnet36;
using HoloToolkit.Sharing;

public class BarMagnetImageTargetSearchEnabler : MonoBehaviour {

    /// <summary>
    /// UserId と OwnerId が違う場合は Vuforia をオフにする
    /// </summary>
	void Start () {
        // まず UserId を取得する
        int myUserId = SharingStage.Instance.Manager.GetLocalUser().GetID();
        //Debug.Log("myUserId: " + myUserId.ToString());

        // 次にこのオブジェクトの OwnerId を取得する
        var syncModelAccessor = gameObject.GetComponent<DefaultSyncModelAccessor>();
        if (syncModelAccessor == null) return;
        int ownerId = ((SyncSpawnedBarMagnetSyncPoint)syncModelAccessor.SyncModel).OwnerId;

        // UserId と OwnerId が違う場合は Vuforia をオフにする
        if (myUserId != ownerId)
        {
            //Debug.Log("[Mismatched] myUserId: " + myUserId.ToString() + ", ownerId: " + ownerId.ToString());
            GameObject.Find("HoloLensCamera").GetComponent<VuforiaBehaviour>().enabled = false;
        }
    }
}

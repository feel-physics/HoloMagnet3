using FeelPhysics.HoloMagnet36;
using HoloToolkit.Sharing;
using UnityEngine;

public class SpawnedBarMagnetSyncPointTransformSetter : MonoBehaviour
{
    private string ObjectTag = "SpawnedBarMagnetSyncPoint01";
    private bool hasLogged = false;
    private GameObject taggedObject = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (taggedObject == null)
        {
            MyHelper.DebugLogEvery10Seconds("Searching tag: " + ObjectTag + " Object", ref hasLogged);
            taggedObject = GameObject.FindWithTag(ObjectTag);
        }
        else
        {
            if (SharingStage.Instance.IsConnected)
            {
                // SpawnedBarMagnetSyncPoint01 の ownerId とmyUserId を比較する
                // もし違うのであれば、returnする

                int myUserId = SharingStage.Instance.Manager.GetLocalUser().GetID();

                var spawnedBMSyncPoint01 = GameObject.FindWithTag("SpawnedBarMagnetSyncPoint01");
                var syncModelAccessor = spawnedBMSyncPoint01.GetComponent<DefaultSyncModelAccessor>();
                if (syncModelAccessor != null)
                {
                    int ownerId = ((SyncSpawnedBarMagnetSyncPoint)syncModelAccessor.SyncModel).OwnerId;
                    //Debug.Log("myUserId = " + myUserId + ", ownerId = " + ownerId);
                    if (myUserId != ownerId) return;
                    //Debug.Log("not returned");
                }
            }

            MyHelper.DebugLogEvery10Seconds(gameObject.name + " setting tag: " +
                ObjectTag + " Object transform: " + transform.position.ToString(), ref hasLogged);
            taggedObject.transform.position = transform.position;
            taggedObject.transform.rotation = transform.rotation;
        }
    }
}
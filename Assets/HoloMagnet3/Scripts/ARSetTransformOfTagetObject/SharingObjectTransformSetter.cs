using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using UnityEngine;

public class SharingObjectTransformSetter : Singleton<SharingObjectTransformSetter>
{

    private GameObject Object;
    private bool IgnoreOnSharing = false;
    private bool hasLogged = false;

    // Use this for initialization
    void Start()
    {
        Object = GameObject.Find("Sharing");
        if (Object != null)
        {
            Object.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Object == null) return;

#if UNITY_EDITOR
        if (!transform.hasChanged) return;
#endif

        // 「Sharing時に無視する && Sharing中」のとき、returnする
        if (IgnoreOnSharing && SharingStage.Instance.IsConnected) return;

        Object.transform.position = transform.position;
        Object.transform.rotation = transform.rotation;
        MyHelper.DebugLogEvery10Seconds("Setting " + Object.name + " Object transform.", ref hasLogged);
    }
}

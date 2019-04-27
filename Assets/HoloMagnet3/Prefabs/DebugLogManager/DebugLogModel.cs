using HoloToolkit.Unity;
using UnityEngine;

public class DebugLogModel : Singleton<DebugLogModel> {
    public GameObject ApplicationDebugLogReference;
    public GameObject SharingDebugLogReference;

    public bool IsShown;

    public void Register(GameObject thisGameObject)
    {
        switch (thisGameObject.name)
        {
            case "ApplicationDebugLog":
                ApplicationDebugLogReference = thisGameObject;
                break;
            case "SharingDebugLog":
                SharingDebugLogReference = thisGameObject;
                break;
            default:
                Debug.LogError("ObjectModel.Register: Not expected name: " + thisGameObject.name);
                break;
        }
    }
}

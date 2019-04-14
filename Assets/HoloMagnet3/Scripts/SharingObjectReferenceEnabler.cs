using HoloToolkit.Unity;
using UnityEngine;

public class SharingObjectReferenceEnabler : Singleton<SharingObjectReferenceEnabler> {
    public GameObject SharingObjectReference = null;

	void Start () {
        SharingObjectReference = gameObject;
    }
}

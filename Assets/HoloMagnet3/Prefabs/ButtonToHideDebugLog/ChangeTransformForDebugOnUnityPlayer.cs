using UnityEngine;

public class ChangeTransformForDebugOnUnityPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
        transform.localPosition = new Vector3(0, 0.4f, 0);
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}
}

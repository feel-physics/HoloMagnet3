using UnityEngine;
using Vuforia;

public class SetInactiveVuforiaOnUnityPlayer : MonoBehaviour {

    // Use this for initialization
    void Awake () {
#if UNITY_EDITOR
        gameObject.GetComponent<VuforiaBehaviour>().enabled = false;
#endif
    }
}

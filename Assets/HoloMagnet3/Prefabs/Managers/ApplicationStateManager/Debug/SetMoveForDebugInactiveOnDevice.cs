using UnityEngine;

public class SetMoveForDebugInactiveOnDevice : MonoBehaviour {

    public GameObject ObjectToInactivate;

#if !UNITY_EDITOR
    private void Awake()
    {
        ObjectToInactivate.SetActive(false);
    }
#endif
}

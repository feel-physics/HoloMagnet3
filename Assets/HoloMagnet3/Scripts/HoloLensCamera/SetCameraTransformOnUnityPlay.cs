using UnityEngine;

public class SetCameraTransformOnUnityPlay : MonoBehaviour
{

#if UNITY_EDITOR
    // Use this for initialization
    void Start()
    {
        gameObject.transform.position += new Vector3(0, 1, -1);
    }
#endif
}

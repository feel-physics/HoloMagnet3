using UnityEngine;

public class DebugLogRegisterer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        DebugLogModel.Instance.Register(gameObject);
    }
}

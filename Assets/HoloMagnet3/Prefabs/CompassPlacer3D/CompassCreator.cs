using UnityEngine;

public class CompassCreator : MonoBehaviour
{
    Vector3 shiftVector =
#if UNITY_EDITOR
    new Vector3(0, 0, 2);
#else
    new Vector3(0, 0, 2);
#endif
    float pitchCompass = 0.03f;
    int numCompassX = 8;
    int numCompassY = 8;
    int numCompassZ = 6;

    // Use this for initialization
    void Start()
    {
        // 2019/04/26
        // GameObject sharingObject = SharingObjectReferenceEnabler.Instance.SharingObjectReference;

        // TODO: 磁力線を描画してないことをチェックする
        Debug.Log("Instantiate compasses");
        GameObject compass = (GameObject)Resources.Load("Compass180509/Compass3D180509");
        for (int d = 0; d < numCompassZ; d++)
        {
            //for (int h = 0; h < numCompass; h++)
            for (int h = 0; h < numCompassY; h++)
            {
                //for (int w = 0; w < numCompass; w++)
                for (int w = 0; w < numCompassX; w++)
                {
                    var localPositionCompassCloned =
                    new Vector3(
                        pitchCompass * w - (numCompassX - 1.0f) / 2.0f * pitchCompass,  // x軸に対し対称に±方向に方位磁針を並べる
                        pitchCompass * h - (numCompassY - 1.0f) / 2.0f * pitchCompass,  // y軸に対し対称に±方向に方位磁針を並べる
                        pitchCompass * d - numCompassZ / 2.0f * pitchCompass) // z軸方向に方位磁針を並べる
                        + shiftVector;

                    var compassCloned = Instantiate(compass,
                        new Vector3(0, 0, 0), Quaternion.identity);
                    // 2019/04/26
                    //compassCloned.transform.parent = sharingObject.transform;
                    //compassCloned.transform.localPosition = localPositionCompassCloned;
                    compassCloned.transform.position = localPositionCompassCloned;  // 2019/04/26
                }
            }
        }

        Debug.Log("CompassPlacer3D.Destroy()");
        Destroy(gameObject);
    }
}
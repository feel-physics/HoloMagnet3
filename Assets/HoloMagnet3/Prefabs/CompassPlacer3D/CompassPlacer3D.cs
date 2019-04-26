using UnityEngine;

public class CompassPlacer3D : MonoBehaviour
{
    Vector3 shiftVector =
#if UNITY_EDITOR
    new Vector3(0, 0.4f, -1);
#else
    new Vector3(0, 0.3f, -1);
#endif
    float pitchCompass = 0.03f;
    int numCompassX = 8;
    int numCompassY = 8;
    int numCompassZ = 6;

    // Use this for initialization
    void Start()
    {
        GameObject sharingObject = SharingObjectReferenceEnabler.Instance.SharingObjectReference;

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
                        pitchCompass * w - numCompassX / 2.0f * pitchCompass,  // x軸に対し対称に±方向に方位磁針を並べる
                        pitchCompass * h - numCompassY / 2.0f * pitchCompass - 0.185f,  // y軸に対し対称に±方向に方位磁針を並べる
                        pitchCompass * d - numCompassZ / 2.0f * pitchCompass + 1.02f) // z軸方向に方位磁針を並べる
                        + shiftVector;

                    var compassCloned = Instantiate(compass,
                        new Vector3(0, 0, 0), Quaternion.identity);
                    compassCloned.transform.parent = sharingObject.transform;
                    compassCloned.transform.localPosition = localPositionCompassCloned;
                }
            }
        }

        Debug.Log("CompassPlacer3D.Destroy()");
        Destroy(gameObject);
    }
}
using UnityEngine;

public class CompassesCreator : MonoBehaviour
{
    Vector3 shiftVector =
    new Vector3(0, 0, 2);

    float pitchCompass = 0.03f;
    /*
    int numCompassX = 8;
    int numCompassY = 8;
    int numCompassZ = 6;

    int numCompassX = 8;
    int numCompassY = 8;
    int numCompassZ = 1;
    */
    private int numCompassX = 8;
    private int numCompassY = 8;
    private int numCompassZ = 6;

    void Start()
    {
        int sceneId = MySceneManager.Instance.sceneId;
        switch (sceneId)
        {
            case 0:
                return;
            case 1:
                numCompassX = 1;
                numCompassY = 1;
                numCompassZ = 1;
                break;
            case 2:
                numCompassX = 8;
                numCompassY = 8;
                numCompassZ = 1;
                break;
            case 3:
                numCompassX = 8;
                numCompassY = 8;
                numCompassZ = 6;
                break;
            default:
                throw new System.Exception("Invalid sceneId");
        }

        Debug.Log("Instantiate compasses");
        GameObject compass = (GameObject)Resources.Load("Compass180509/Compass3D180509");
        for (int d = 0; d < numCompassZ; d++)
        {
            for (int h = 0; h < numCompassY; h++)
            {
                for (int w = 0; w < numCompassX; w++)
                {
                    var localPositionCompassCloned =
                    new Vector3(
                        pitchCompass * w - (numCompassX - 1.0f) / 2.0f * pitchCompass,  // x軸に対し対称に±方向に方位磁針を並べる
                        pitchCompass * h - (numCompassY - 1.0f) / 2.0f * pitchCompass,  // y軸に対し対称に±方向に方位磁針を並べる
                        pitchCompass * d - (numCompassZ - 1.0f) / 2.0f * pitchCompass) // z軸方向に方位磁針を並べる
                        + shiftVector;

                    var compassCloned = Instantiate(compass,
                        new Vector3(0, 0, 0), Quaternion.identity);
                    compassCloned.transform.position = localPositionCompassCloned;  // 2019/04/26
                }
            }
        }

        Debug.Log("CompassPlacer3D.Destroy()");
    }
}
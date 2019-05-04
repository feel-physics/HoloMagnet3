using UnityEngine;

public class CompassesCreator : MonoBehaviour
{
    Vector3 shiftVector = new Vector3(0, 0, 2);

    float pitchCompass;

    private int numCompassX = 1;
    private int numCompassY = 1;
    private int numCompassZ = 1;

    private enum Dimensiton { D2, D3 };
    private Dimensiton dimensiton;

    void Start()
    {
        MySceneManager.MySceneEnum scene = MySceneManager.Instance.MyScene;
        switch (scene)
        {
            case MySceneManager.MySceneEnum.Introduction:
                return;
            case MySceneManager.MySceneEnum.Compass_One:
                numCompassX = 1;
                numCompassY = 1;
                numCompassZ = 1;
                break;
            case MySceneManager.MySceneEnum.Compasses_2D:
                numCompassX = 8;
                numCompassY = 8;
                numCompassZ = 1;
                dimensiton = Dimensiton.D2;
                pitchCompass = 0.07f;
                break;
            case MySceneManager.MySceneEnum.Compasses_3D:
                numCompassX = 8;
                numCompassY = 8;
                numCompassZ = 6;
                dimensiton = Dimensiton.D3;
                pitchCompass = 0.07f;
                break;
            default:
                throw new System.Exception("Invalid sceneId");
        }

        Debug.Log("Instantiate compasses");  // Todo: 10秒おきのログの文面を現在進行形にする
        GameObject compass;
        switch (dimensiton)
        {
            case Dimensiton.D2:
                compass = (GameObject)Resources.Load("Compass180509/Compass2D180509");
                break;
            case Dimensiton.D3:
                compass = (GameObject)Resources.Load("Compass180509/Compass3D180509");
                break;
            default:
                throw new System.Exception("Invalid dimension");
        }
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
                        pitchCompass * d - (numCompassZ - 1.0f) / 2.0f * pitchCompass)  // z軸に対し対称に±方向に方位磁針を並べる
                        + shiftVector;

                    var compassCloned = Instantiate(compass,
                        new Vector3(0, 0, 0), Quaternion.identity);
                    compassCloned.transform.position = localPositionCompassCloned;  // 2019/04/26
                }
            }
        }
    }
}
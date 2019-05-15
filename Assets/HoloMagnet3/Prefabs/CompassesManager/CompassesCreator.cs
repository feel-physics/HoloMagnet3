using UnityEngine;

public class CompassesCreator : MonoBehaviour
{
    //初期配置位置のオフセット（カメラの2メートル先）
    Vector3 shiftVector = new Vector3(0, 0, 2);

    //コンパスを配置する間隔
    float pitchCompass;

    private int numCompassX = 1;
    private int numCompassY = 1;
    private int numCompassZ = 1;

    private enum Dimensiton { D2, D3 };
    private Dimensiton dimensiton;

    void Start()
    {
        // Introductionのシーンでは方位磁針を生成しない
        if (MySceneManager.Instance.MyScene ==
            MySceneManager.MySceneEnum.Introduction)
        {
            return;
        }

        SetupCompassesPlacement();

        Debug.Log("Instantiate compasses");

        SetupCompassesParent();

        //次元に合わせて、コンパスのPrefabを設定する
        GameObject compass = LoadCompassPrefab();

        //CompasのSharedMaterialを取得する
        RegisterSharedmaterialToCompassesModel(compass);

        //コンパスをループで生成する
        CreateCompasses(compass);
    }

    void SetupCompassesPlacement()
    {
        MySceneManager.MySceneEnum scene = MySceneManager.Instance.MyScene;

        //シーンに合わせて、配置するコンパス数を設定する
        //Introduction シーンの場合は、処理を停止する
        // Todo: Compass_Oneシーンの場合は、ObjectInitializationで位置を調節する
        switch (scene)
        {
            case MySceneManager.MySceneEnum.Compass_One:
                numCompassX = 1;
                numCompassY = 1;
                numCompassZ = 1;
                break;
            case MySceneManager.MySceneEnum.Compasses_2D:
                numCompassX = 12;
                numCompassY = 12;
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
                shiftVector = new Vector3(0, -0.7f, 2);
                break;
            default:
                throw new System.Exception("Invalid sceneId");
        }
    }

    void SetupCompassesParent()
    {
        //Compassの親のTransformを生成して、CompassModelに登録する
        var parent = new GameObject();
        // CompassParentの初期位置はCompassManagerと同じ
        parent.transform.position = transform.position;
        parent.name = "CommpassesParent";
        CompassesModel.Instance.ParentTransform = parent.transform;
        CompassesModel.Instance.pitch = pitchCompass;
    }

    GameObject LoadCompassPrefab()
    {
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

        return compass;
    }

    void RegisterSharedmaterialToCompassesModel(GameObject compass)
    {
        var mats = compass.GetComponentInChildren<MeshRenderer>().sharedMaterials;

        CompassesModel.Instance.MatNorth = mats[0];
        CompassesModel.Instance.MatSouth = mats[1];
    }

    void CreateCompasses(GameObject compass)
    {
        for (int d = 0; d < numCompassZ; d++)
        {
            for (int h = 0; h < numCompassY; h++)
            {
                for (int w = 0; w < numCompassX; w++)
                {
                    // 初期位置の設定
                    // CompassesManagerを中心に配置する
                    var localPositionCompassCloned =
                    new Vector3(
                        pitchCompass * w - (numCompassX - 1.0f) / 2.0f * pitchCompass,  // x軸に対し対称に±方向に方位磁針を並べる
                        pitchCompass * h - (numCompassY - 1.0f) / 2.0f * pitchCompass,  // y軸に対し対称に±方向に方位磁針を並べる
                        pitchCompass * d - (numCompassZ - 1.0f) / 2.0f * pitchCompass)  // z軸に対し対称に±方向に方位磁針を並べる
                        + transform.position;

                    //Instantiate
                    // 親に子として付ける
                    //var compassCloned = Instantiate(compass, localPositionCompassCloned, Quaternion.identity, CompassesModel.Instance.ParentTransform);
                    var compassCloned = Instantiate(
                        compass, 
                        localPositionCompassCloned, 
                        Quaternion.identity,
                        CompassesModel.Instance.ParentTransform);
                    //名前を付ける
                    compassCloned.name = string.Format("Compass_{0}-{1}-{2}", w, h, d);
                }
            }
        }
    }
}
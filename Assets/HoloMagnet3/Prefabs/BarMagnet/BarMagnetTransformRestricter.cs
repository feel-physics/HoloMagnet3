using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMagnetTransformRestricter : MonoBehaviour
{
    // 棒磁石を以下のz座標に固定する
    private float zRestricted = 0.7f;
    // Start時にシーンを取得し、Updateで使い回す
    private enum Scene { Introduction, OneCompass, Compass2D, Compass3D};
    private Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        MySceneManager.MySceneEnum sceneReference = MySceneManager.MyScene;

        //シーンに合わせて、配置するコンパス数を設定する
        //Introduction シーンの場合は、処理を停止する
        switch (sceneReference)
        {
            case MySceneManager.MySceneEnum.Introduction:
                scene = Scene.Introduction;
                break;
            case MySceneManager.MySceneEnum.Compass_One:
                scene = Scene.OneCompass;
                break;
            case MySceneManager.MySceneEnum.Compasses_2D:
                scene = Scene.Compass2D;
                break;
            case MySceneManager.MySceneEnum.Compasses_3D:
                scene = Scene.Compass3D;
                break;
            default:
                throw new System.Exception("Invalid sceneId");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (scene)
        {
            case Scene.Introduction:
                RotateOnlyAroundZAxis();
                break;
            case Scene.OneCompass:
                RotateOnlyAroundZAxis();
                break;
            case Scene.Compass2D:
                RotateOnlyAroundZAxis();
                RestrictZ();
                break;
            case Scene.Compass3D:
                break;
            default:
                throw new System.Exception("Invalid sceneId");
        }

    }

    private void RestrictZ()
    {
        Vector3 pos = transform.position;
        Vector3 newpos = new Vector3(pos.x, pos.y, zRestricted);
        transform.position = newpos;
    }

    private void RotateOnlyAroundZAxis()
    {
        Vector3 rot = gameObject.transform.localEulerAngles;
        Quaternion newQuo = Quaternion.Euler(new Vector3(0, 0, rot.z));
        transform.rotation = newQuo;
    }
}

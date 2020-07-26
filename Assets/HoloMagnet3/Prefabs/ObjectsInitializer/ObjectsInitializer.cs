using UnityEngine;

/// <summary>
/// Unityでビルドしなくても、実機で確かめながら位置などを微調整できる
/// MySceneManagerから呼ばれる
/// </summary>
public class ObjectsInitializer
{

    string logMessage = " was initialized";

    public void Initialize()
    {
        InitializeBarMagnetAndCompassesManager();
    }

    private void InitializeBarMagnetAndCompassesManager()
    {
        // オブジェクトを取得
        GameObject barMagnet01 = GameObject.Find("BarMagnet01");

        // 初期位置を設定
        Vector3 initialPosition = new Vector3(0, 0, 2);
        if (MySceneManager.MyScene ==
            MySceneManager.MySceneEnum.Compasses_3D)
        {
            initialPosition = new Vector3(0, -0.7f, 2);
        }
        barMagnet01.transform.position = initialPosition;

        // ログ
        Debug.Log(barMagnet01.name + logMessage);


        //compassesManagerは廃止
        //GameObject compassesManager =
        //    GameObject.Find("CompassesManager");
        //compassesManager.transform.position = initialPosition;
        //Debug.Log(compassesManager.name + logMessage);
    }
}

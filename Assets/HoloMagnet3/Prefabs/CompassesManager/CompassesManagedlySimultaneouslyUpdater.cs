using System.Collections.Generic;
using UnityEngine;

// Todo: 後でクラス名をRenameする
public class CompassesManagedlySimultaneouslyUpdater : MonoBehaviour
{
    [SerializeField]
    private BarMagnetModel magnet;  // Todo: 後でbarMagnet01Model に変える
                                    // Todo: 後でbarMagnet01.northPoleになるようにする（松井さん？）

    [SerializeField]
    private GameObject barMagnet01NorthPole;
    [SerializeField]
    private GameObject barMagnet01SouthPole;

    [SerializeField]
    private GameObject barMagnet02NorthPole;
    [SerializeField]
    private GameObject barMagnet02SouthPole;

    // Todo: 後で確認する Unity - Manual: Debugging DirectX 11/12 shaders with Visual Studio https://docs.unity3d.com/Manual/SL-DebuggingD3D11ShadersWithVS.html
    // Todo: 後でCompassesManagedlySimultaneouslyUpdaterをCompassesManagedlyUpdaterにRename
    // Todo: 後でBarMagnetMagneticForceLinesSimultaneouslyDrawerからSimultaneouslyを削除
    // Todo: 読み上げるテキストを作る
    // Todo: テキストを読み上げてくれるサービスを探す
    // Todo: コメントを英訳するかは相談する
    // Todo: Storeに上げる準備（アイコンなど）をする（うまくいかないときはHoloMagnet3.oldをそのまま持ってくる）
    // Todo: Storeに上げる
    // Todo: Storeの画像を変える
    // Todo: 後で動的に二次元シーン開始時にBarMagnetRistrictMovementをアタッチする

    // Todo: 今後：磁力線をインスタンス化しない頂点シェーダで描く

    // 明るさの係数
    [SerializeField]
    private float brightnessCoefficient = 0.005f;
    //private float brightnessCoefficient = 0.0002f;
    // 3次元用の明るさの係数
    [SerializeField]
    private float brightnessCoefficient3D = 0.0002f;
    //private float brightnessCoefficient3D = 0.0002f;

    // 明るさの下限
    [SerializeField]
    private float brightnessLowerLimit = 0.04f;

    CompassesModel compassesModel;
    private void Start()
    {
        compassesModel = GetComponent<CompassesModel>();
        SetupForChangingBrightness();

        //Invoke("SetupForChangingBrightness", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // Observer
        var compasses =
            compassesModel.CompassesReferenceForManagedlyUpdate;

        if (compasses.Count > 0)  // Todo: IntroductionではCompassesManagerを消す
        {
            //磁石の移動に合わせて位置をupdateする  Todo: 後で説明をメソッドのサマリーに移す
            UpdateCompassParentPosition();

            //コンパスの向きのupdate
            ManagedlyUpdate(compasses);
        }
    }

    void UpdateCompassParentPosition()
    {
        //コンパスの間隔何個分ずれたら、移動を発生させるかの閾値
        int check = 1;

        //磁石とコンパスの位置差分を取得
        var offset = magnet.transform.position -
            compassesModel.ParentTransform.position;
        //コンパスの間隔を取得
        var pitch = compassesModel.pitch;

        //間隔何個分ずれているかを取得
        var vecToMove = offset / pitch;


        //x/y/zの優先順位で適用する
        var p = Vector3.zero;

        if (Mathf.Abs(vecToMove.x) >= check)
        {
            p.x += (int)vecToMove.x;
        }

        if (Mathf.Abs(vecToMove.y) >= check)
        {
            p.y += (int)vecToMove.y;
        }

        if (Mathf.Abs(vecToMove.z) >= check)
        {
            p.z += (int)vecToMove.z;
        }

        if (p != Vector3.zero)
        {
            //compassesModel.ParentTransform.position += p * pitch;
            var lastpos = compassesModel.ParentTransform.position;
            lastpos = new Vector3(
                pitch * (int)(lastpos.x / pitch),
                pitch * (int)(lastpos.y / pitch),
                pitch * (int)(lastpos.z / pitch)
            );

            compassesModel.ParentTransform.position = lastpos + (p * pitch);
        }
    }

    void ManagedlyUpdate(List<CompassManagedlyUpdater> compasses)
    {
        //コンパスが存在しているシーンでは、コンパスシェーダーにmaginetの位置を登録する
        if (compassesModel.MatNorth != null)
        {
            AssignMagnetPosition();
        }

/*        foreach (CompassManagedlyUpdater compass in compasses)
        {
            compass.ManagedlyUpdate();
        }*/
        for (int i = 0; i < compasses.Count; i ++)
        {
            compasses[i].ManagedlyUpdate();
        }
    }

    void SetupForChangingBrightness()
    {

        magnet = transform.parent.GetComponent<BarMagnetModel>();

        try
        {
            GameObject barMagnet01 = GameObject.Find("BarMagnet01");
            barMagnet01NorthPole = barMagnet01.transform.Find("North Body/North Pole").gameObject;
            barMagnet01SouthPole = barMagnet01.transform.Find("South Body/South Pole").gameObject;
        }
        catch
        {
            Debug.Log("Could not find BarMagnet01.");
        }

        try
        {
            GameObject barMagnet02 = GameObject.Find("BarMagnet02");
            barMagnet02NorthPole = barMagnet02.transform.Find("North Body/North Pole").gameObject;
            barMagnet02SouthPole = barMagnet02.transform.Find("South Body/South Pole").gameObject;
        }
        catch
        {
            Debug.Log("Could not find BarMagnet02.");
        }


        // 3次元の場合は3次元用の明るさの係数を使う
        if (MySceneManager.MyScene == MySceneManager.MySceneEnum.Compasses_3D)
        {
            brightnessCoefficient = brightnessCoefficient3D;
        }

        // Todo: 今後以下のN極とS極で分かれている記述をまとめる
        // Todo: できればマテリアルをまとめてしまいたい
        // 方位磁針の明るさの係数
        compassesModel.MatNorth.SetFloat(
            "_BrightnessCoefficient", brightnessCoefficient);
        compassesModel.MatSouth.SetFloat(
            "_BrightnessCoefficient", brightnessCoefficient);
        // 方位磁針の明るさの下限
        compassesModel.MatNorth.SetFloat(
            "_BrightnessLowerLimit", brightnessLowerLimit);
        compassesModel.MatSouth.SetFloat(
            "_BrightnessLowerLimit", brightnessLowerLimit);
    }

    void AssignMagnetPosition()
    {
        // Todo: 以下のN極とS極で分かれている記述をまとめる

        //var p = magnet.transform.position;
        var np1 = barMagnet01NorthPole.transform.position;
        var sp1 = barMagnet01SouthPole.transform.position;
        var n1v4 = new Vector4(np1.x, np1.y, np1.z, 0);  // Convert to Vector4
        var s1v4 = new Vector4(sp1.x, sp1.y, sp1.z, 0);  // Convert to Vector4

        // Set coordinates to Shader of Material of NORTH side of compass

        compassesModel.MatNorth.SetVector("_NorthPole1Pos", n1v4);
        compassesModel.MatNorth.SetVector("_SouthPole1Pos", s1v4);

        // Set coordinates to Shader of Material of SOUTH side of compass
        compassesModel.MatSouth.SetVector("_NorthPole1Pos", n1v4);
        compassesModel.MatSouth.SetVector("_SouthPole1Pos", s1v4);


        if (barMagnet02NorthPole != null)
        {

            var np2 = barMagnet02NorthPole.transform.position;
            var sp2 = barMagnet02SouthPole.transform.position;

            var n2v4 = new Vector4(np2.x, np2.y, np2.z, 0);  // Convert to Vector4
            var s2v4 = new Vector4(sp2.x, sp2.y, sp2.z, 0);  // Convert to Vector4

            // Set coordinates to Shader of Material of NORTH side of compass
            compassesModel.MatNorth.SetVector("_NorthPole2Pos", n2v4);
            compassesModel.MatNorth.SetVector("_SouthPole2Pos", s2v4);
            compassesModel.MatSouth.SetVector("_NorthPole2Pos", n2v4);
            compassesModel.MatSouth.SetVector("_SouthPole2Pos", s2v4);
        }
        //磁石2が無い場合は、磁石2用のパラメタに磁石１の値を設定する
        else
        {
            compassesModel.MatNorth.SetVector("_NorthPole2Pos", n1v4);
            compassesModel.MatNorth.SetVector("_SouthPole2Pos", s1v4);
            compassesModel.MatSouth.SetVector("_NorthPole2Pos", n1v4);
            compassesModel.MatSouth.SetVector("_SouthPole2Pos", s1v4);
        }
    }
}

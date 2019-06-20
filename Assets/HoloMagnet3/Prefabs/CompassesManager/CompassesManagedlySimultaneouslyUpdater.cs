﻿using System.Collections.Generic;
using UnityEngine;

// Todo: 後でクラス名をRenameする
public class CompassesManagedlySimultaneouslyUpdater : MonoBehaviour
{
    private BarMagnetModel magnet;  // Todo: 後でbarMagnet01Model に変える
    // Todo: 後でbarMagnet01.northPoleになるようにする（松井さん？）
    private GameObject barMagnet01NorthPole;
    private GameObject barMagnet01SouthPole;

    private GameObject barMagnet02NorthPole;
    private GameObject barMagnet02SouthPole;
    // Todo: 後で確認する Unity - Manual: Debugging DirectX 11/12 shaders with Visual Studio https://docs.unity3d.com/Manual/SL-DebuggingD3D11ShadersWithVS.html
    // Todo: 後でCompassesManagedlySimultaneouslyUpdaterをCompassesManagedlyUpdaterにRename
    // Todo: 後でBarMagnetMagneticForceLinesSimultaneouslyDrawerからSimultaneouslyを削除
    // Todo: 読み上げるテキストを作る
    // Todo: テキストを読み上げてくれるサービスを探す
    // Todo: コメントを英訳するかは相談する
    // Todo: Storeの画像を変える
    // Todo: 後で動的に二次元シーン開始時にBarMagnetRistrictMovementをアタッチする

    // Todo: 今後：磁力線をインスタンス化しない頂点シェーダで描く

    // 明るさの係数
    [SerializeField]
    private float brightnessCoefficient = 0.005f;
    // 3次元用の明るさの係数
    [SerializeField]
    private float brightnessCoefficient3D = 0.002f;

    // 明るさの下限
    [SerializeField]
    private float brightnessLowerLimit = 0.04f;

    private void Start()
    {
        SetupForChangingBrightness();
    }

    // Update is called once per frame
    void Update()
    {
        // Observer
        var compasses = 
            CompassesModel.Instance.CompassesReferenceForManagedlyUpdate;

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
            CompassesModel.Instance.ParentTransform.position;
        //コンパスの間隔を取得
        var pitch = CompassesModel.Instance.pitch;

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
            CompassesModel.Instance.ParentTransform.position += p * pitch;
    }

    void ManagedlyUpdate(List<CompassManagedlyUpdater> compasses)
    {
        //コンパスが存在しているシーンでは、コンパスシェーダーにmaginetの位置を登録する
        if (CompassesModel.Instance.MatNorth != null)
        {
            AssignMagnetPosition();
        }

        foreach (CompassManagedlyUpdater compass in compasses)
        {
            compass.ManagedlyUpdate();
        }
    }

    void SetupForChangingBrightness()
    {
        magnet = FindObjectOfType<BarMagnetModel>();
        GameObject barMagnet01 = GameObject.Find("BarMagnet01");
        barMagnet01NorthPole = barMagnet01.transform.Find("North Body/North Pole").gameObject;
        barMagnet01SouthPole = barMagnet01.transform.Find("South Body/South Pole").gameObject;

        GameObject barMagnet02 = GameObject.Find("BarMagnet02");
        barMagnet02NorthPole = barMagnet02.transform.Find("North Body/North Pole").gameObject;
        barMagnet02SouthPole = barMagnet02.transform.Find("South Body/South Pole").gameObject;

        // 3次元の場合は3次元用の明るさの係数を使う
        if (MySceneManager.Instance.MyScene == MySceneManager.MySceneEnum.Compasses_3D)
        {
            brightnessCoefficient = brightnessCoefficient3D;
        }

        // Todo: 今後以下のN極とS極で分かれている記述をまとめる
        // Todo: できればマテリアルをまとめてしまいたい
        // 方位磁針の明るさの係数
        CompassesModel.Instance.MatNorth.SetFloat(
            "_BrightnessCoefficient", brightnessCoefficient);
        CompassesModel.Instance.MatSouth.SetFloat(
            "_BrightnessCoefficient", brightnessCoefficient);
        // 方位磁針の明るさの下限
        CompassesModel.Instance.MatNorth.SetFloat(
            "_BrightnessLowerLimit", brightnessLowerLimit);
        CompassesModel.Instance.MatSouth.SetFloat(
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
        CompassesModel.Instance.MatNorth.SetVector("_NorthPole1Pos", n1v4);
        CompassesModel.Instance.MatNorth.SetVector("_SouthPole1Pos", s1v4);
        // Set coordinates to Shader of Material of SOUTH side of compass
        CompassesModel.Instance.MatSouth.SetVector("_NorthPole1Pos", n1v4);
        CompassesModel.Instance.MatSouth.SetVector("_SouthPole1Pos", s1v4);
    }
}

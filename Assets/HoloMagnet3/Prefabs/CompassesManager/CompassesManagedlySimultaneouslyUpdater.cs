using System.Collections.Generic;
using UnityEngine;

public class CompassesManagedlySimultaneouslyUpdater : MonoBehaviour
{
    private BarMagnetModel magnet;  // Todo: barMagnet01Model に変える
    // Todo: barMagnet01.northPoleになるようにする（松井さん？）
    private GameObject barMagnet01NorthPole;
    private GameObject barMagnet01SouthPole;
    // Todo: Unity - Manual: Debugging DirectX 11/12 shaders with Visual Studio https://docs.unity3d.com/Manual/SL-DebuggingD3D11ShadersWithVS.html
    // Todo: CompassesManagedlySimultaneouslyUpdaterをCompassesManagedlyUpdaterにRename
    // Todo: BarMagnetMagneticForceLinesSimultaneouslyDrawerからSimultaneouslyを削除
    // Todo: 2次元のシーンで棒磁石と方位磁針のz座標がズレる問題を解消
    // Todo: つかんでいる手のシェーダーをMRTK/standard に変える
    // Todo: Compass_Oneのシーンの方位磁針の位置に印を作る
    // Todo: シーン遷移の音を鳴らす
    // Todo: 読み上げるテキストを作る
    // Todo: テキストを読み上げてくれるサービスを探す
    // Todo: コメントを英訳するかは相談する
    // Todo: Storeに上げる準備（アイコンなど）をする（うまくいかないときはHoloMagnet3.oldをそのまま持ってくる）
    // Todo: Storeに上げる
    // Todo: Storeの画像を変える

    private void Start()
    {
        magnet = FindObjectOfType<BarMagnetModel>();
        GameObject barMagnet01 = GameObject.Find("BarMagnet01");
        barMagnet01NorthPole = barMagnet01.transform.Find("North Body/North Pole").gameObject;
        barMagnet01SouthPole = barMagnet01.transform.Find("South Body/South Pole").gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        // Observer
        var compasses = CompassesModel.Instance.CompassesReferenceForManagedlyUpdate;

        // Presenter
        if (compasses.Count == 0)
            return;

        //コンパスが複数の場合は、磁石の移動に合わせて位置をupdateする
        if (compasses.Count > 1)
            UpdateCompassParentPosition();

        //コンパスの向きのupdate
        ManagedlyUpdate(compasses);
    }

    void UpdateCompassParentPosition()
    {
        //コンパスの間隔何個分ずれたら、移動を発生させるかの閾値
        int check = 1;

        //磁石とコンパスの位置差分を取得
        var offset = magnet.transform.position - CompassesModel.Instance.ParentTransform.position;
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
            //var p = magnet.transform.position;
            var np = barMagnet01NorthPole.transform.position;
            var sp = barMagnet01SouthPole.transform.position;
            var nv4 = new Vector4(np.x, np.y, np.z, 0);  //Vector4 に変換
            var sv4 = new Vector4(sp.x, sp.y, sp.z, 0);  //Vector4 に変換

            // 方位磁針の N 極側のマテリアルのシェーダに座標をセット
            CompassesModel.Instance.MatNorth.SetVector("_NorthPolePos", nv4);
            CompassesModel.Instance.MatNorth.SetVector("_SouthPolePos", sv4);
            // 方位磁針の S 極側のマテリアルのシェーダに座標をセット
            CompassesModel.Instance.MatSouth.SetVector("_NorthPolePos", nv4);
            CompassesModel.Instance.MatSouth.SetVector("_SouthPolePos", sv4);
        }

        foreach (CompassManagedlyUpdater compass in compasses)
        {
            compass.ManagedlyUpdate();
        }
    }
}

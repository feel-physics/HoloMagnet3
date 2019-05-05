using HoloToolkit.Unity;
using UnityEngine;

public class BarMagnetMagneticForceLineDrawer : Singleton<BarMagnetMagneticForceLineDrawer> {

    // Todo: これは何だ？Sharing用のようだが…
    float scaleToFitLocalPosition = 0.15f;

    // --- 線分の長さ ---
    // Todo: この長さと数を調節してN極から出た磁力線とS極から出た磁力線が一致するようにする
    float baseLengthOfLine = 0.1f;
    int numLines = 100;

    /// <summary>
    /// 引数の(x, y, z)を始点として磁力線を描く
    /// </summary>
    public void Draw(GameObject magnetForceLine, bool lineIsFromNorthPole, Vector3 startPosition, float width)
    {
        /*
         * その時点のすべてのN極、S極をタグを使って取得する
         * HoloMagnet3では棒磁石は1つしかないが、HoloMagnet5では最低でも2つになる
         * HoloMagnet5では、アプリケーションの途中から磁石の数が変わる可能性があるため、毎フレーム取得する
         * Todo: 本当はMagnetsManagerクラスを作り、MagnetManagerModelが変わったらeventで反映させるようにしたい
         */
        GameObject[] northPoles = GameObject.FindGameObjectsWithTag("North Pole");
        GameObject[] southPoles = GameObject.FindGameObjectsWithTag("South Pole");

        // LineRendererを準備する
        LineRenderer line = magnetForceLine.GetComponent<LineRenderer>();

        // === LineRendererを設定する ===
        // --- LineRendererを初期化する ---
        line.useWorldSpace = true;　　// Sharingのときのため。HoloMagnet5で使用。
        line.positionCount = numLines;  // 描く線分の数

        // --- LineRendererの始点を初期位置にセットする ---
        line.SetPosition(0, startPosition);  // 引数の(x, y, z)を始点として磁力線を描く

        // --- lineの長さ ---
        float lengthOfLine = baseLengthOfLine * scaleToFitLocalPosition;

        // --- lineの太さ ---
        line.startWidth = width;
        line.endWidth = width;

        // === 変数の準備 ===

        Vector3 positionCurrentPoint = startPosition;

        // 線分を描画し続ける
        for (int i = 1; i < line.positionCount; i++)
        {
            Vector3 forceResultant = MagneticForceCaliculator.Instance.ForceResultant(
                northPoles, southPoles, positionCurrentPoint);

            // --- 描画 ---
            if (lineIsFromNorthPole)
            {
                positionCurrentPoint += forceResultant.normalized * lengthOfLine;
            }
            else
            {
                positionCurrentPoint += -forceResultant.normalized * lengthOfLine;
            }

            line.SetPosition(i, positionCurrentPoint);
        }
    }
}

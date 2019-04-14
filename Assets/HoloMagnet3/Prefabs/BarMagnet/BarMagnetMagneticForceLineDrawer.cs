using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

public class BarMagnetMagneticForceLineDrawer : Singleton<BarMagnetMagneticForceLineDrawer> {

    // Todo: Listがわからない
    //private List<GameObject> northPolesList;
    //private List<GameObject> southPolesList;

    /// <summary>
    /// 引数の(x, y, z)を始点として磁力線を描く
    /// </summary>
    public void Draw(GameObject magnetForceLine, bool lineIsFromNorthPole, Vector3 startPosition, float width)
    {
        // すべてのN極、S極を取得する
        GameObject[] northPoles = GameObject.FindGameObjectsWithTag("North Pole");
        GameObject[] southPoles = GameObject.FindGameObjectsWithTag("South Pole");

        // すべてのN極、S極を取得する
        // アプリケーションの途中から磁石の数が変わる可能性があるため、毎フレーム取得する
        // 本当はMagnetsManagerクラスを作り、MagnetManagerModelが変わったらeventで反映させるようにしたい
        // Todo: Listがわからない
        // northPolesList が null のため
        // "Object reference not set to an instance of an object" エラーが出て動かない
        /*
        northPolesList.Clear();
        northPolesList.Add(BarMagnetModel.Instance.NorthPoleReference);
        southPolesList.Clear();
        southPolesList.Add(BarMagnetModel.Instance.SouthPoleReference);
        */

        // LineRendererを準備する
        LineRenderer line = magnetForceLine.GetComponent<LineRenderer>();

        // === LineRendererを設定する ===
        // --- LineRendererを初期化する ---
        line.useWorldSpace = true;
        line.positionCount = 100;  // 描く線分の数

        // --- LineRendererの始点を初期位置にセットする ---
        line.SetPosition(0, startPosition);  // 引数の(x, y, z)を始点として磁力線を描く

        // --- lineの太さ ---
        line.startWidth = width;
        line.endWidth = width;

        // === 変数の準備 ===

        Vector3 positionCurrentPoint = startPosition;

        float scaleToFitLocalPosition = 0.15f;

        // 線分を描画し続ける
        for (int i = 1; i < line.positionCount; i++)
        {
            //Vector3 forceResultant = ForceResultant(northPoles, southPoles, positionCurrentPoint);
            Vector3 forceResultant = MagneticForceCaliculator.Instance.ForceResultant(
                northPoles, southPoles, positionCurrentPoint);

            // --- 線分の長さ ---
            float lengthOfLine = 0.01f * scaleToFitLocalPosition;

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

            /*
            // --- OpenGL ---
            // Draw lines
            GL.Begin(GL.LINES);

            GL.Color(new Color(1, 0, 0, 1));
            GL.Vertex3(0, 0, 0.7f);
            GL.Vertex3(positionCurrentPoint.x, positionCurrentPoint.y, positionCurrentPoint.z);

            GL.Color(new Color(0, 1, 0, 1));
            GL.Vertex3(0.1F, 0.1F, 0);
            GL.Vertex3(0, 0.1F, 0);

            GL.End();
            // --- end of OpenGL ---
            */
        }
    }
}

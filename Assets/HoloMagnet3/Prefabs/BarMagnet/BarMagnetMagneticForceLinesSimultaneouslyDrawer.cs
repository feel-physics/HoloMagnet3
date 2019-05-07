#undef elapsed_time  // 磁力線を引く処理時間を計測するため
using HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// UpdateでApplicationParamsのstateを監視
/// </summary>
public class BarMagnetMagneticForceLinesSimultaneouslyDrawer : Singleton<BarMagnetMagneticForceLinesSimultaneouslyDrawer>
{

    public int Mode = 0;  // 0: 2D, 1: 3D  Todo: Listを使う
    private GameObject magneticForceLinePrefab;

    private bool hasLogged;

    static Material lineMaterial;

    int dimension = 2;
    private bool _IsDrawing = false;
    public bool IsDrawing
    {
        set
        {
            _IsDrawing = value;
            if (!value)
                DeleteLines();

        }
        get
        {
            return _IsDrawing;
        }
    }
    private void Start()
    {
        magneticForceLinePrefab = BarMagnetModel.Instance.MagneticForceLinePrefab;

        MySceneManager.MySceneEnum scene = MySceneManager.Instance.MyScene;
        if (scene == MySceneManager.MySceneEnum.Compasses_3D)
        {
            dimension = 3;
        }
    }

    public void Update()
    {
        if (IsDrawing)
            DrawLoop(dimension);
    }

    public void DeleteLines()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("CloneLine");

        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
    }

    public void DrawLoop(int dimension)
    {
        GameObject myMagnet = gameObject;

        this.DeleteLines();

        Vector3 myBarMagnetNorthPoleWorldPosition = BarMagnetModel.Instance.NorthPoleReference.transform.position;
        Vector3 myBarMagnetSouthPoleWorldPosition = BarMagnetModel.Instance.SouthPoleReference.transform.position;

        // デバッグ用ログ出力
        MyHelper.DebugLogEvery10Seconds(
            "DrawMagnetForceLines3D.Update() is fired.\n" +
            "BarMagnet: " + gameObject.transform.position.ToString() + "\n" +
            "NorthPole: " + myBarMagnetNorthPoleWorldPosition.ToString() + "\n" +
            "SouthPole: " + myBarMagnetSouthPoleWorldPosition.ToString(), ref hasLogged);

        Vector3 barMagnetDirection = transform.rotation.eulerAngles;

        for (int i = -1; i <= 1; i += 2)  // j=1のときN極側、j=-1のときS極側の磁力線を描く
        {
            int numStartY;
            int numEndY;
            int numShiftY;
            int numStartZ;
            int numEndZ;
            int numShiftZ;

            if (dimension == 3)
            {
                numStartY = -2;  // 磁力線描画開始地点、y方向
                numEndY = -numStartY;  // 磁石の中心に対して対称に描画開始地点を配置する
                numShiftY = 2;   // 磁力線描画開始地点間の長さ、y方向

                numStartZ = -2;  // 磁力線描画開始地点、z方向
                numEndZ = -numStartZ;
                numShiftZ = 1;   // 磁力線描画開始地点間の長さ、z方向
            }
            else if (dimension == 2)
            {
                numStartY = -2;
                numEndY = -numStartY;
                numShiftY = 1;

                numStartZ = 0;
                numEndZ = -numStartZ;
                numShiftZ = 1;
            }
            else
            {
                throw new System.Exception("Invalid Mode");
            }

            for (int indexY = numStartY; indexY <= numEndY; indexY += numShiftY) // z
            {
                for (int indexZ = numStartZ; indexZ <= numEndZ; indexZ += numShiftZ) // y
                {
                    GameObject magneticForceLine =
                        Instantiate(magneticForceLinePrefab, transform.position, Quaternion.identity);

                    // 作成したオブジェクトを子として登録
                    magneticForceLine.tag = "CloneLine";
                    magneticForceLine.transform.parent = transform;

                    bool lineIsFromNorthPole = true;
                    Vector3 myBarMagnetPoleWorldPosition;

                    // N極
                    if (i == 1)
                    {
                        lineIsFromNorthPole = true;
                        myBarMagnetPoleWorldPosition = myBarMagnetNorthPoleWorldPosition;
                    }
                    // S極
                    else
                    {
                        lineIsFromNorthPole = false;
                        myBarMagnetPoleWorldPosition = myBarMagnetSouthPoleWorldPosition;
                    }

                    Vector3 shiftPositionFromMyPole = new Vector3(
                        0.001f * indexY,  // y
                        0.001f * i,  // x
                        0.001f * indexZ  // z
                        );

                    shiftPositionFromMyPole =
                        myMagnet.transform.rotation * shiftPositionFromMyPole;
                    Vector3 startPosition = myBarMagnetPoleWorldPosition + shiftPositionFromMyPole;

#if elapsed_time
                    // 処理時間の計測
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
#endif
                    DrawOne(magneticForceLine, lineIsFromNorthPole, startPosition, 0.003f);
#if elapsed_time
                    // 処理時間の計測
                    sw.Stop();

                    Debug.Log("DrawMagnetForceLines3D takes " + sw.ElapsedMilliseconds + "ms");
#endif
                }
            }
        }

    }

    float scaleToFitLocalPosition = 0.15f;

    // --- 線分の長さ ---
    // Todo: この長さを調節してN極から出た磁力線とS極から出た磁力線が一致するようにする
    float baseLengthOfLine = 0.1f;

    // Todo: Listがわからない
    //private List<GameObject> northPolesList;
    //private List<GameObject> southPolesList;

    /// <summary>
    /// 引数の(x, y, z)を始点として磁力線を描く
    /// </summary>
    public void DrawOne(GameObject magnetForceLine, bool lineIsFromNorthPole, Vector3 startPosition, float width)
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
            //Vector3 forceResultant = ForceResultant(northPoles, southPoles, positionCurrentPoint);
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

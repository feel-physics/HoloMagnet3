#undef elapsed_time  // 磁力線を引く処理時間を計測するため
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 磁力線の描画を行う
/// </summary>
public class BarMagnetMagneticForceLinesSimultaneouslyDrawer : Singleton<BarMagnetMagneticForceLinesSimultaneouslyDrawer>
{
    //public int Mode = 0;  // 0: 2D, 1: 3D  Todo: Listを使う
    //磁力線描画のPrefab
    private GameObject magneticForceLinePrefab;
    //ログ出力用
    private bool hasLogged;

    static Material lineMaterial;

    /// <summary>
    /// 磁力線を描画するサイズを定義するクラス
    /// </summary>
    public class DimensionScale
    {
        /// <summary>
        /// 磁力線描画開始地点
        /// </summary>
        public int start { private set; get; }

        /// <summary>
        /// 磁力線描画終了地点
        /// </summary>
        public int end { private set; get; }

        /// <summary>
        /// 磁力線描画開始地点間の長さ
        /// </summary>
        public int shift { private set; get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_start"> 磁力線描画開始地点</param>
        /// <param name="_end">磁力線描画終了地点</param>
        /// <param name="_shift">磁力線描画開始地点間の長さ</param>
        public DimensionScale(int _start, int _end, int _shift)
        {
            start = _start;
            end = _end;
            shift = _shift;
        }
    }

    public int dimension = 2;
    private DimensionScale scaleY;
    private DimensionScale scaleZ;

    /// <summary>
    /// 磁力線を描画中か管理するフラグの実態(private)
    /// </summary>
    [SerializeField] private bool _IsDrawing = false;

    /// <summary>
    /// 磁力線を描画中か管理するフラグ(public)
    /// </summary>
    public bool IsDrawing
    {
        set
        {
            Debug.Log(value);
            _IsDrawing = value;

            //描画中止への変更を検知して、DeleteLines()を呼び出す
            if (!value)
            {
                DeleteLines();
            }
        }
        get
        {
            return _IsDrawing;
        }
    }

    List<LineRenderer> magneticForceLines = null;

    private void Start()
    {
        magneticForceLinePrefab = BarMagnetModel.Instance.MagneticForceLinePrefab;

        magneticForceLines = new List<LineRenderer>();

        MySceneManager.MySceneEnum scene = MySceneManager.Instance.MyScene;
        if (scene == MySceneManager.MySceneEnum.Compasses_3D)
        {
            dimension = 3;
            scaleY = new DimensionScale(-2, 2, 2);
            scaleZ = new DimensionScale(-2, 2, 1);
        }
        else
        {
            dimension = 2;
            scaleY = new DimensionScale(-2, 2, 1);
            scaleZ = new DimensionScale(0, 0, 1);
        }
    }

    public void Update()
    {
        if (IsDrawing)
        {
#if elapsed_time
            // 処理時間の計測
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            //必要なら磁力線の初期化
            if (magneticForceLines.Count == 0)
                GenerateLines(scaleY, scaleZ);

            //N極磁力線の描画
            DrawLoop(dimension, true, BarMagnetModel.Instance.NorthPoleReference.transform.position);
           
            //S極磁力線の描画
            DrawLoop(dimension, false, BarMagnetModel.Instance.SouthPoleReference.transform.position);

#if elapsed_time
            // 処理時間の計測
            sw.Stop();

            Debug.Log("DrawMagnetForceLines3D takes " + sw.ElapsedMilliseconds + "ms");
#endif

        }
    }


    public void GenerateLines(DimensionScale y, DimensionScale z)
    {
        if (magneticForceLines.Count > 0)
        {
            DeleteLines();
        }

        for (int indexY = y.start; indexY <= y.end; indexY += y.shift) // z
        {
            for (int indexZ = z.start; indexZ <= z.end; indexZ += z.shift) // y
            {
                //Debug.Log(string.Format("Y:{0} Z:{1}", indexY, indexZ));

                //N用とS用とLoop毎に２つ生成する

                var g = Instantiate(magneticForceLinePrefab, transform.position, Quaternion.identity);
                // 作成したオブジェクトを子として登録
                g.tag = "CloneLine";
                g.transform.parent = transform;
                var line = g.GetComponent<LineRenderer>();
                magneticForceLines.Add(line);

                g = Instantiate(magneticForceLinePrefab, transform.position, Quaternion.identity);
                // 作成したオブジェクトを子として登録
                g.tag = "CloneLine";
                g.transform.parent = transform;
                line = g.GetComponent<LineRenderer>();
                magneticForceLines.Add(line);

            }
        }
        Debug.Log("GenerateLines:" + magneticForceLines.Count);
    }

    public void DeleteLines()
    {
        foreach (var line in magneticForceLines)
        {
            Destroy(line.gameObject);
        }
        magneticForceLines.Clear();
    }

    public void DrawLoop(int dimension, bool lineIsFromNorthPole, Vector3 polePosInWorld)
    {
        // デバッグ用ログ出力
        MyHelper.DebugLogEvery10Seconds(
            "DrawMagnetForceLines3D.Update() is firing.\n" +
            "BarMagnet: " + gameObject.transform.position.ToString() + "\n" +
            "Pole: " + lineIsFromNorthPole.ToString() + "\n", ref hasLogged);

        Vector3 barMagnetDirection = transform.rotation.eulerAngles;

        int cnt = (lineIsFromNorthPole ? 0 : (int)magneticForceLines.Count / 2);

        for (int indexY = scaleY.start; indexY <= scaleY.end; indexY += scaleY.shift) // z
        {
            for (int indexZ = scaleZ.start; indexZ <= scaleZ.end; indexZ += scaleZ.shift) // y
            {
                Vector3 shiftPositionFromMyPole = new Vector3(
                    0.001f * indexY,  // y
                    0.001f * (lineIsFromNorthPole ? 1 : -1),  // x
                    0.001f * indexZ  // z
                    );

                shiftPositionFromMyPole =
                    gameObject.transform.rotation * shiftPositionFromMyPole;
                Vector3 startPosition = polePosInWorld + shiftPositionFromMyPole;

                DrawOne(magneticForceLines[cnt], lineIsFromNorthPole, startPosition, 0.003f);

                cnt++;
            }
        }

    }

    float scaleToFitLocalPosition = 0.15f;

    // --- 線分の長さ ---
    // Todo: この長さを調節してN極から出た磁力線とS極から出た磁力線が一致するようにする
    [SerializeField] float baseLengthOfLine = 0.1f;

    // Todo: Listがわからない
    //private List<GameObject> northPolesList;
    //private List<GameObject> southPolesList;

    /// <summary>
    /// 引数の(x, y, z)を始点として磁力線を描く
    /// </summary>
    public void DrawOne(LineRenderer magnetForceLine, bool lineIsFromNorthPole, Vector3 startPosition, float width)
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



        // === LineRendererを設定する ===
        // --- LineRendererを初期化する ---
        magnetForceLine.useWorldSpace = true;
        magnetForceLine.positionCount = 50;  // 描く線分の数

        // --- LineRendererの始点を初期位置にセットする ---
        magnetForceLine.SetPosition(0, startPosition);  // 引数の(x, y, z)を始点として磁力線を描く

        // --- lineの長さ ---
        float lengthOfLine = baseLengthOfLine * scaleToFitLocalPosition;

        // --- lineの太さ ---
        magnetForceLine.startWidth = width;
        magnetForceLine.endWidth = width;

        // === 変数の準備 ===

        Vector3 positionCurrentPoint = startPosition;

        // 線分を描画し続ける
        for (int i = 1; i < magnetForceLine.positionCount; i++)
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

            magnetForceLine.SetPosition(i, positionCurrentPoint);

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

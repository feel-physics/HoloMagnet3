#undef elapsed_time  // 磁力線を引く処理時間を計測するため
using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Todo: 後でクラス名をRenameする
// Todo: 後でStartとUpdateからメソッドを外出しする
/// <summary>
/// 磁力線の描画を行う
/// </summary>
public class BarMagnetMagneticForceLinesDrawer : MonoBehaviour
{
    //磁力線描画のPrefab
    private GameObject magneticForceLinePrefab;
    //ログ出力用
    private bool hasLogged;

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

            // 音を鳴らす  Todo: 棒磁石移動の音と一緒に再生されてしまう
            audioSource.clip = acDraw;
            audioSource.loop = false;
            audioSource.Play();
#if false
            MyHelper.MyDelayMethod(this, 1f, () =>
            {
                audioSource.clip = null;
            });
#endif

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
    List<float> listStartY;
    List<float> listStartZ;

    [SerializeField]
    private AudioClip acDraw;
    private AudioSource audioSource;

    Transform[] southPolesTransform;
    Transform[] northPolesTransform;

    long phase1Total = 0;
    long phase2Total = 0;
    int numOfCalclation = 0;

    BarMagnetModel barMagnetModel;
    private void Start()
    {
        barMagnetModel = GetComponent<BarMagnetModel>();
        magneticForceLinePrefab = barMagnetModel.MagneticForceLinePrefab;

        magneticForceLines = new List<LineRenderer>();

        listStartY = new List<float> { -0.02f, -0.002f, 0, 0.002f, 0.02f };

        var scene = MySceneManager.Instance.MyScene;

        if (scene == MySceneManager.MySceneEnum.Compasses_3D ||
            scene == MySceneManager.MySceneEnum.TwoBarMagnets)
        {
            listStartZ = new List<float> { -0.002f, 0, 0.002f };
        }
        else
        {
            listStartZ = new List<float> { 0 };
        }

        audioSource = GetComponents<AudioSource>()[0];
    }

    public void Update()
    {
        if (IsDrawing)
        {
#if elapsed_time
            // 処理時間の計測
            phase1Total = 0;
            phase2Total = 0;
            numOfCalclation = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            //必要なら磁力線の初期化
            if (magneticForceLines.Count == 0)
                GenerateLines();

            //N極磁力線の描画
            DrawLoop(true, barMagnetModel.NorthPoleReference.transform.position);

            //S極磁力線の描画
            DrawLoop(false, barMagnetModel.SouthPoleReference.transform.position);

#if elapsed_time
            // 処理時間の計測
            sw.Stop();

            //Debug.Log("DrawMagnetForceLines3D takes " + sw.ElapsedMilliseconds + "ms");
            var phase2elapsed = phase2Total - phase1Total;
            var phase1nano = ((double)phase1Total / System.Diagnostics.Stopwatch.Frequency) * 1000000000 / numOfCalclation;
            var phase2nano = ((double)phase2elapsed / System.Diagnostics.Stopwatch.Frequency) * 1000000000 / numOfCalclation;
            Debug.Log(string.Format("phase1: {0}nsec, phase2elapsed: {1}nsec", phase1nano, phase2nano));
            Debug.Log(string.Format("MagneticForceCalculator.Instance.ForceResultant: {0}nsec, phase2elapsed: {1}nsec", phase1nano, phase2nano));

#endif

        }
    }

    private void InitializeLineRenderer(LineRenderer magnetForceLine)
    {
        // === LineRendererを設定する ===
        // --- LineRendererを初期化する ---
        magnetForceLine.useWorldSpace = true;
        magnetForceLine.positionCount = numLines;

        // --- lineの太さ ---
        magnetForceLine.startWidth = widthLines;
        magnetForceLine.endWidth = widthLines;
    }

    public void GenerateLines()
    {
        if (magneticForceLines.Count > 0)
        {
            DeleteLines();
        }

        foreach (float startY in listStartY)
        {
            foreach (float startZ in listStartZ)
            {
                //N用とS用とLoop毎に２つ生成する

                var g = Instantiate(magneticForceLinePrefab, transform.position, Quaternion.identity);
                // 作成したオブジェクトを子として登録
                g.tag = "CloneLine";
                g.transform.parent = transform;
                var line = g.GetComponent<LineRenderer>();
                InitializeLineRenderer(line);
                magneticForceLines.Add(line);

                g = Instantiate(magneticForceLinePrefab, transform.position, Quaternion.identity);
                // 作成したオブジェクトを子として登録
                g.tag = "CloneLine";
                g.transform.parent = transform;
                line = g.GetComponent<LineRenderer>();
                InitializeLineRenderer(line);
                magneticForceLines.Add(line);
            }
        }

        // すべてのN極、S極を取得する
        northPolesTransform = GameObject.FindGameObjectsWithTag("North Pole").
            Select(go => go.transform).
            ToArray();
        southPolesTransform = GameObject.FindGameObjectsWithTag("South Pole").
            Select(go => go.transform).
            ToArray();
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

    public void DrawLoop(bool lineIsFromNorthPole, Vector3 polePosInWorld)
    {
        // デバッグ用ログ出力
        MyHelper.DebugLogEvery10Seconds(
            "DrawLoop() of BarMagnetForceLinesDrawer is firing.\n" +
            "BarMagnet: " + gameObject.transform.position.ToString() + "\n" +
            "Pole: " + lineIsFromNorthPole.ToString() + "\n", ref hasLogged);

        Vector3 barMagnetDirection = transform.rotation.eulerAngles;

        int cnt = (lineIsFromNorthPole ? 0 : (int)magneticForceLines.Count / 2);

        var rotation = gameObject.transform.rotation;
        foreach (float startY in listStartY)
        {
            foreach (float startZ in listStartZ)
            {
                //var  shiftPositionFromMyPole2 = gameObject.transform.rotation * shiftPositionFromMyPole;
                //Vector3 startPosition = polePosInWorld + shiftPositionFromMyPole2;

                Vector3 shiftPositionFromMyPole = new Vector3(
                    startY,
                    0.001f * (lineIsFromNorthPole ? 1 : -1),  // 極からx方向にどれくらい離すか
                    startZ
                    );

                DrawOne(
                    magneticForceLines[cnt],
                    lineIsFromNorthPole,
                    polePosInWorld + rotation * shiftPositionFromMyPole);

                cnt++;
            }
        }

    }

    float scaleToFitLocalPosition = 0.15f;

    /// <summary>
    /// 線分の長さ
    /// 調節してN極から出た磁力線とS極から出た磁力線が一致するようにする
    /// </summary>
    [SerializeField] float baseLengthOfLine = 0.02f;

    /// <summary>
    /// 描く線分の数
    /// </summary>
    [SerializeField] int numLines = 110;

    /// <summary>
    /// 描く線分の太さ
    /// 調節してN極から出た磁力線とS極から出た磁力線が一致するようにする
    /// </summary>
    [SerializeField] float widthLines = 0.005f;

    private readonly System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    /// <summary>
    /// 引数の(x, y, z)を始点として磁力線を描く
    /// </summary>
    public void DrawOne(LineRenderer magnetForceLine, bool lineIsFromNorthPole, Vector3 startPosition)
    {
        // --- LineRendererの始点を初期位置にセットする ---
        magnetForceLine.SetPosition(0, startPosition);  // 引数の(x, y, z)を始点として磁力線を描く

        // --- lineの長さ ---
        float lengthOfLine = baseLengthOfLine * scaleToFitLocalPosition;

        // === 変数の初期化 ===
        Vector3 positionCurrentPoint = startPosition;

        // 線分を描画し続ける
        for (int i = 1; i < magnetForceLine.positionCount; i++)
        {
            sw.Reset();
            sw.Start();

            Vector3 forceResultant = MagneticForceCalculator.Instance.ForceResultant(
                MyHelper.ToPoleArray(northPolesTransform),
                MyHelper.ToPoleArray(southPolesTransform),
                positionCurrentPoint);


            phase1Total += sw.ElapsedTicks;

            // --- 描画 ---
            if (lineIsFromNorthPole)
            {
                positionCurrentPoint += forceResultant.normalized * lengthOfLine;
            }
            else
            {
                positionCurrentPoint += -forceResultant.normalized * lengthOfLine;
            }

            phase2Total += sw.ElapsedTicks;
            numOfCalclation++;

            sw.Stop();
            magnetForceLine.SetPosition(i, positionCurrentPoint);
        }


#if false
        MyHelper.DebugLogEvery10Seconds(
            "phase1: {phase1Total.ToString()}, " +
            "phase2: {phase2Total.ToString()}", 
            ref hasLogged);
#endif
    }
}

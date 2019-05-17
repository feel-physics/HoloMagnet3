#undef elapsed_time  // 磁力線を引く処理時間を計測するため
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

// Todo: 後でクラス名をRenameする
// Todo: 後でStartとUpdateからメソッドを外出しする
/// <summary>
/// 磁力線の描画を行う
/// </summary>
public class BarMagnetMagneticForceLinesSimultaneouslyDrawer : Singleton<BarMagnetMagneticForceLinesSimultaneouslyDrawer>
{
    //磁力線描画のPrefab
    private GameObject magneticForceLinePrefab;
    //ログ出力用
    private bool hasLogged;

    static Material lineMaterial;

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

    private void Start()
    {
        magneticForceLinePrefab = BarMagnetModel.Instance.MagneticForceLinePrefab;

        magneticForceLines = new List<LineRenderer>();

        listStartY = new List<float> { -0.02f, -0.002f, 0, 0.002f, 0.02f };

        MySceneManager.MySceneEnum scene = MySceneManager.Instance.MyScene;
        if (scene == MySceneManager.MySceneEnum.Compasses_3D)
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
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            //必要なら磁力線の初期化
            if (magneticForceLines.Count == 0)
                GenerateLines();

            //N極磁力線の描画
            DrawLoop(true, BarMagnetModel.Instance.NorthPoleReference.transform.position);
           
            //S極磁力線の描画
            DrawLoop(false, BarMagnetModel.Instance.SouthPoleReference.transform.position);

#if elapsed_time
            // 処理時間の計測
            sw.Stop();

            Debug.Log("DrawMagnetForceLines3D takes " + sw.ElapsedMilliseconds + "ms");
#endif

        }
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

    public void DrawLoop(bool lineIsFromNorthPole, Vector3 polePosInWorld)
    {
        // デバッグ用ログ出力
        MyHelper.DebugLogEvery10Seconds(
            "DrawLoop() of BarMagnetForceLinesDrawer is firing.\n" +
            "BarMagnet: " + gameObject.transform.position.ToString() + "\n" +
            "Pole: " + lineIsFromNorthPole.ToString() + "\n", ref hasLogged);

        Vector3 barMagnetDirection = transform.rotation.eulerAngles;

        int cnt = (lineIsFromNorthPole ? 0 : (int)magneticForceLines.Count / 2);

        foreach (float startY in listStartY)
        {
            foreach (float startZ in listStartZ)
            {
                Vector3 shiftPositionFromMyPole = new Vector3(
                    startY,
                    0.001f * (lineIsFromNorthPole ? 1 : -1),  // 極からx方向にどれくらい離すか
                    startZ
                    );

                shiftPositionFromMyPole =
                    gameObject.transform.rotation * shiftPositionFromMyPole;
                Vector3 startPosition = polePosInWorld + shiftPositionFromMyPole;

                DrawOne(magneticForceLines[cnt], lineIsFromNorthPole, startPosition);

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

    /// <summary>
    /// 引数の(x, y, z)を始点として磁力線を描く
    /// </summary>
    public void DrawOne(LineRenderer magnetForceLine, bool lineIsFromNorthPole, Vector3 startPosition)
    {
        // すべてのN極、S極を取得する
        GameObject[] northPoles = GameObject.FindGameObjectsWithTag("North Pole");
        GameObject[] southPoles = GameObject.FindGameObjectsWithTag("South Pole");

        // === LineRendererを設定する ===
        // --- LineRendererを初期化する ---
        magnetForceLine.useWorldSpace = true;
        magnetForceLine.positionCount = numLines;

        // --- LineRendererの始点を初期位置にセットする ---
        magnetForceLine.SetPosition(0, startPosition);  // 引数の(x, y, z)を始点として磁力線を描く

        // --- lineの長さ ---
        float lengthOfLine = baseLengthOfLine * scaleToFitLocalPosition;

        // --- lineの太さ ---
        magnetForceLine.startWidth = widthLines;
        magnetForceLine.endWidth = widthLines;

        // === 変数の初期化 ===
        Vector3 positionCurrentPoint = startPosition;

        // 線分を描画し続ける
        for (int i = 1; i < magnetForceLine.positionCount; i++)
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

            magnetForceLine.SetPosition(i, positionCurrentPoint);
        }
    }
}

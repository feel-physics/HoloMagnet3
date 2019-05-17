/*
 * 参考にさせて頂きました：
 * MRTKを用いてHoloLensのダブルタップを検出する \- MRが楽しい
 * http://bluebirdofoz.hatenablog.com/entry/2019/02/06/060929
 */

 using UnityEngine;
// IInputClickHandler を利用するため InputModule を追加
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class MultiTapHandler : Singleton<MultiTapHandler>,
IInputClickHandler // タップ操作検出
{
    /// <summary>
    /// 連続タップ許容時間(秒)
    /// </summary>
    [SerializeField, Tooltip("連続タップ許容時間(秒)")]
    private float MultTapTime = 0.5f;

    /// <summary>
    /// Hold判定時間(秒)
    /// </summary>
    [SerializeField, Tooltip("Hold判定時間(秒)")]
    private float HoldTime = 1.0f;

    /// <summary>
    /// Hold効果音
    /// </summary>
    [SerializeField, Tooltip("Hold効果音")]
    private AudioClip ACHold;

    /// <summary>
    /// Finish効果音
    /// </summary>
    [SerializeField, Tooltip("Finish効果音")]
    private AudioClip ACFinish;

    /// <summary>
    /// Hold開始時刻
    /// </summary>
    private float p_HoldStart;

    /// <summary>
    /// Holdが終了したか否か
    /// </summary>
    private bool holdEnded;

    /// <summary>
    /// 連続タップカウント
    /// </summary>
    private int p_MultTapCount;

    /// <summary>
    /// 連続タップ計測開始時刻
    /// </summary>
    private float p_MultTapStart;
    private float tapTime;

    AudioSource audioSource;

    /// <summary>
    /// 起動時処理
    /// </summary>
    void Start()
    {
        // FallBackEventHandlerにする
        InputManager.Instance.PushFallbackInputHandler(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 定期実行
    /// </summary>
    void Update()
    {
        // 初回タップ
        if (p_MultTapCount == 1)
        {
            // 経過時間確認
            float elapsedTime = Time.time - p_MultTapStart;
            // 初回タップから規定時間経過しても次のタップが来ない場合、
            // シングルタップと判定
            if (p_MultTapCount == 1 && MultTapTime < elapsedTime)
            {
                // --- シングルタップ処理 ---
                // 磁力線描画処理のオンオフを切り替える
                BarMagnetMagneticForceLinesSimultaneouslyDrawer.Instance.IsDrawing = !BarMagnetMagneticForceLinesSimultaneouslyDrawer.Instance.IsDrawing;

                // 終了処理
                p_MultTapCount = 0;
                p_HoldStart = 0;
                holdEnded = false;
            }
        }
        // 連続タップ
        else if(p_MultTapCount > 1)
        {
            // タップカウントが 2 以上の時、連続タップの発生チェック
            if ((Time.time - p_MultTapStart) > MultTapTime)
            {
                // 連続タップ許容時間が経過していればカウントに応じて処理を実行
                if (p_MultTapCount == 2)
                {
                    // --- ダブルタップ処理 ---
                    // 次のシーンをロード
                    MySceneManager.Instance.LoadNextScene();
                }
                // 終了処理
                p_MultTapCount = 0;
                p_HoldStart = 0;
                holdEnded = false;
            }
        }
        // Hold開始
        else if (p_HoldStart != 0 &&
            (Time.time - p_HoldStart) > HoldTime)
        {
            // Hold開始音を鳴らす
            audioSource.clip = ACHold;
            audioSource.loop = false;
            audioSource.Play();

            // 終了処理
            p_MultTapCount = 0;
            p_HoldStart = 0;
            holdEnded = false;
        }
        // Hold終了
        else if (holdEnded)
        {
            audioSource.Stop();

            audioSource.clip = ACFinish;
            audioSource.loop = false;
            audioSource.Play();

            // 終了処理
            holdEnded = false;
        }
    }

    /// <summary>
    /// タップ検出
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // 現在時刻の取得
        float nowTime = Time.time;

        // 連続タップ確認
        tapTime = nowTime - p_MultTapStart;
        if (tapTime > MultTapTime)
        {
            // 前回タップから連続タップ許容時間を超えていれば初回タップと再判定
            p_MultTapCount = 1;
        }
        else
        {
            // 前回タップから連続タップ許容時間内ならば連続タップと判定
            p_MultTapCount++;
        }
        // 連続タップ計測開始時刻を更新
        p_MultTapStart = nowTime;
    }

    public void OnManipulationStarted()
    {
        // 現在時刻の取得
        p_HoldStart = Time.time;
    }

    public void OnManipulationEnded()
    {
        holdEnded = true;
    }
}

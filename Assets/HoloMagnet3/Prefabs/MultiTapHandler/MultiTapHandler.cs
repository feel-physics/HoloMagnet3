/*
 * 参考にさせて頂きました：
 * MRTKを用いてHoloLensのダブルタップを検出する \- MRが楽しい
 * http://bluebirdofoz.hatenablog.com/entry/2019/02/06/060929
 */

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// IInputClickHandler を利用するため InputModule を追加
using HoloToolkit.Unity.InputModule;

public class MultiTapHandler : MonoBehaviour,
IInputClickHandler // タップ操作検出
{
    /// <summary>
    /// 連続タップ許容時間(秒)
    /// </summary>
    [SerializeField, Tooltip("連続タップ許容時間(秒)")]
    private float MultTapTime = 0.5f;

    /// <summary>
    /// 連続タップカウント
    /// </summary>
    private int p_MultTapCount;

    /// <summary>
    /// 連続タップ計測開始時刻
    /// </summary>
    private float p_MultTapStart;

    /// <summary>
    /// 起動時処理
    /// </summary>
    void Start()
    {
        // FallBackEventHandlerにする
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    /// <summary>
    /// 定期実行
    /// </summary>
    void Update()
    {
        // 連続タップ判定
        if (p_MultTapCount > 1)
        {
            // タップカウントが 2 以上の時、連続タップの発生チェック
            if ((Time.time - p_MultTapStart) > MultTapTime)
            {
                // 連続タップ許容時間が経過していればカウントに応じて処理を実行
                if (p_MultTapCount == 2)
                {
                    // ダブルタップ処理
                    Debug.Log("DoubleTap");
                    MySceneManager.Instance.LoadNextScene();
                }
                p_MultTapCount = 0;
            }
        }
        // タップカウントが1のとき
        else if (p_MultTapCount == 1)
        {
            // 磁力線描画処理のオンオフを切り替える
            BarMagnetModel.Instance.IsDrawing = !BarMagnetModel.Instance.IsDrawing;
            // 終了処理
            p_MultTapCount = 0;
        }
    }

    /// <summary>
    /// タップ検出
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("clicked!");

        // 現在時刻の取得
        float nowTime = Time.time;

        // 連続タップ確認
        float tapTime = nowTime - p_MultTapStart;
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
}

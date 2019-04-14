using FeelPhysics.HoloMagnet36;
using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// SpawnedApplicationManagerのモデルが変化したら、
/// ApplicationManagerに通知する
/// </summary>
public class SpawnedDebugLogModelSender : Singleton<SpawnedDebugLogModelSender>
{
    // SpawnedApplicationDebugLogModelの変化を毎フレームチェックする
    private bool isShownCurrent = false;
    private bool isShownOld = true;

    // ※ISyncValueAccessable
    DefaultSyncModelAccessor syncModelAccessor = null;

    // Use this for initialization
    void Start()
    {
        // ※ISyncValueAccessable
        syncModelAccessor = gameObject.GetComponent<DefaultSyncModelAccessor>();
        if (syncModelAccessor != null)
        {
            Debug.Log("syncModelAccessor doesn't exist");
        }
    }

    /// <summary>
    /// 変更があれば送信する
    /// </summary>
    private void Update()
    {
        // 現在のシェアリングアプリケーション状態を取得する
        isShownCurrent =
            ((SyncSpawnedDebugLogManager)GetComponent<DefaultSyncModelAccessor>().SyncModel).
             IsShown.Value;

        // SpawnedApplicationModelが変化していたら…
        if (isShownCurrent != isShownOld)
        {
            Send();
        }

        // --- 終了処理 ---
        // Applicationの状態を保存する
        isShownOld = isShownCurrent;
    }

    void Send()
    {
        switch (isShownCurrent)
        {
            case true:
                DebugLogModel.Instance.IsShown = true;
                break;
            case false:
                DebugLogModel.Instance.IsShown = false;
                break;
            default:
                break;
        }

        //applicationDebugLogWriter.AnchorDebugText.text += "\nSent sharingApplicationStateCurrent: " + sharingApplicationStateCurrent;
    }
}

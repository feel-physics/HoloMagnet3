using FeelPhysics.HoloMagnet36;
using HoloToolkit.Sharing;
using UnityEngine;

/// <summary>
/// SpawnedApplicationManagerのモデルが変化したら、
/// ApplicationManagerに通知する
/// </summary>
public class SpawnedApplicationStateSender : MonoBehaviour {

    // SpawnedApplicationModelの変化を毎フレームチェックする
    private int stateCurrent = 0;
    private int stateOld = 0;
    private bool sharingIsInitializedCurrent = true;
    private bool sharingIsInitializedOld = true;

    // ※ISyncValueAccessable
    DefaultSyncModelAccessor syncModelAccessor = null;

    // Use this for initialization
    void Start () {
        // ※ISyncValueAccessable
        syncModelAccessor = gameObject.GetComponent<DefaultSyncModelAccessor>();
        if (syncModelAccessor != null)
        {
            Debug.Log("syncModelAccessor doesn't exist");
        }

        // シェアリング参加時に初期化済みであることを送信する
        ApplicationStateModel.Instance.SharingIsInitialized = true;
    }

    /// <summary>
    /// 変更があれば送信する
    /// </summary>
    private void Update()
    {
        getCurrentModel();

        // SpawnedApplicationModelが変化していたら…
        if (stateCurrent != stateOld || sharingIsInitializedCurrent != sharingIsInitializedOld)
        {
            Send();
        }

        // --- 終了処理 ---
        // Applicationの状態を保存する
        stateOld = stateCurrent;
        sharingIsInitializedOld = sharingIsInitializedCurrent;
    }

    void Send()
    {
        switch (stateCurrent)
        {
            case 0:
                ApplicationStateModel.Instance.state =
                    ApplicationStateModel.State.ShowNothing;
                break;
            case 10:
                ApplicationStateModel.Instance.state =
                    ApplicationStateModel.State.SharingInitializing;
                break;
            case 20:
                ApplicationStateModel.Instance.state =
                    ApplicationStateModel.State.DrawLines;
                break;
            case 30:
                ApplicationStateModel.Instance.state =
                    ApplicationStateModel.State.ShowCompass;
                break;
            default:
                break;
        }

        switch (sharingIsInitializedCurrent)
        {
            case true:
                ApplicationStateModel.Instance.SharingIsInitialized = true;
                break;
            case false:
                ApplicationStateModel.Instance.SharingIsInitialized = false;
                break;
            default:
                ApplicationStateModel.Instance.SharingIsInitialized = true;
                break;
        }

        //applicationDebugLogWriter.AnchorDebugText.text += "\nSent sharingApplicationStateCurrent: " + sharingApplicationStateCurrent;
    }

    void getCurrentModel()
    {
        // 現在のシェアリングアプリケーション状態を取得する
        stateCurrent =
            ((SyncSpawnedApplicationStateManager)GetComponent<DefaultSyncModelAccessor>().SyncModel).
            ApplicationState.Value;

        // 現在のアプリケーションがシェアリング初期化済みか取得する
        sharingIsInitializedCurrent =
            ((SyncSpawnedApplicationStateManager)GetComponent<DefaultSyncModelAccessor>().SyncModel).
            SharingIsInitialized.Value;
    }
}

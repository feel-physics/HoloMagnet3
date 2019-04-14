using FeelPhysics.HoloMagnet36;
using HoloToolkit.Sharing;
using HoloToolkit.Unity;

/// <summary>
/// シェアリングApplicationManagerのモデルを操作する
/// </summary>
public class SpawnedDebugLogModelReceiver : Singleton<SpawnedDebugLogModelReceiver>
{
    /// <summary>
    /// ローカルのApplicationDebugLogModelの変更に従って、SpawnedApplicationModelを変更する
    /// </summary>
    /// <param name="localState"></param>
    public void Receive(bool localIsShown)
    {
        // シェアリングしているアプリケーションの状態変数を用意する
        // ※spawnedApplicationManagerのまま
        var SpawnedApplicationModel =
            (SyncSpawnedDebugLogManager)GetComponent<DefaultSyncModelAccessor>().SyncModel;

        // ローカルのデバッグログの表示状態を、シェアリングしているアプリケーション状態変数に適用する
        bool isShownValue;
        switch (localIsShown)
        {
            case true:
                isShownValue = true;
                break;
            case false:
                isShownValue = false;
                break;
            default:
                isShownValue = false;
                break;
        }

        SpawnedApplicationModel.IsShown.Value = isShownValue;
    }
}

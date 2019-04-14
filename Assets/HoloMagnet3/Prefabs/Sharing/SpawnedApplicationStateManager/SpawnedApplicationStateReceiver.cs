using FeelPhysics.HoloMagnet36;
using HoloToolkit.Sharing;
using HoloToolkit.Unity;

/// <summary>
/// シェアリングApplicationManagerのモデルを操作する
/// </summary>
public class SpawnedApplicationStateReceiver : Singleton<SpawnedApplicationStateReceiver> {

    /// <summary>
    /// ローカルのApplicationModelの変更に従って、SpawnedApplicationModelを変更する
    /// </summary>
    public void Receive (ApplicationStateModel.State localState, bool localSharingIsInisialized) {
        // シェアリングしているアプリケーションの状態変数を用意する
        var SpawnedApplicationModel = 
            (SyncSpawnedApplicationStateManager)GetComponent<DefaultSyncModelAccessor>().SyncModel;

        // ローカルのアプリケーションの状態を、シェアリングしているアプリケーション状態変数に適用する
        int stateValue;
        switch (localState)
        {
            case ApplicationStateModel.State.ShowNothing:
                stateValue = 0;
                break;
            case ApplicationStateModel.State.SharingInitializing:
                stateValue = 10;
                break;
            case ApplicationStateModel.State.DrawLines:
                stateValue = 20;
                break;
            case ApplicationStateModel.State.ShowCompass:
                stateValue = 30;
                break;
            default:
                stateValue = 999;
                break;
        }

        SpawnedApplicationModel.ApplicationState.Value = stateValue;
        SpawnedApplicationModel.SharingIsInitialized.Value = true;
    }
}

using UnityEngine;

/// <summary>
/// ローカルのApplicationParamを監視して、状態が変化したときに必要な処理を指示する
/// </summary>
public class ApplicationStatePresenter : MonoBehaviour {

    // BarMagnet01への参照を取得する準備
    private GameObject barMagnet01 = null;
    private string barMagnet01ObjectName = "BarMagnet01";

    // ApplicationStateを操作する準備
    private ApplicationStateModel.State applicationStateOld = ApplicationStateModel.State.ShowNothing;
    private ApplicationStateModel.State applicationStateCurrent = ApplicationStateModel.State.ShowNothing;

    // SharingIsInitializedを操作する準備
    private bool sharingIsInitializedOld = false;
    private bool sharingIsInitializedCurrent = false;

    // ※ApplicationDebugLogWriter
    private ApplicationDebugLogWriter applicationDebugLogWriter = null;

    // 監視する変数への参照を取得する
    void Start () {
        // ApplicationStateへの参照を取得する
        applicationStateOld = ApplicationStateModel.Instance.state;
        // BarMagnet01への参照を取得する
        barMagnet01 = GameObject.Find(barMagnet01ObjectName);

        // ※ApplicationDebugLogWriter
        applicationDebugLogWriter = GetComponent<ApplicationDebugLogWriter>();
    }

    // Update is called once per frame
    void Update () {
        // 現在のApplicationの状態を取得する
        applicationStateCurrent = ApplicationStateModel.Instance.state;
        sharingIsInitializedCurrent = ApplicationStateModel.Instance.SharingIsInitialized;

        // ApplicationManagerの状態が変化していたら該当処理を行う
        // Applicationの状態が変化していたら…
        if (applicationStateCurrent != applicationStateOld || 
            sharingIsInitializedCurrent != sharingIsInitializedOld)
        {
            Present();
        }

        // --- 終了処理 ---
        // Applicationの状態を保存する
        applicationStateOld = applicationStateCurrent;
        sharingIsInitializedOld = sharingIsInitializedCurrent;
    }

    void Present()
    {
        if (SpawnedApplicationManagerExists())
        {
            SpawnedApplicationStateReceiver.Instance.Receive(
                applicationStateCurrent, sharingIsInitializedCurrent);
        }

        switch (applicationStateCurrent)
        {
            case ApplicationStateModel.State.ShowNothing:
                BarMagnetModel.Instance.IsDrawing = false;
                CompassesModel.Instance.IsShown = false;
                break;
            case ApplicationStateModel.State.SharingInitializing:
                SpawnerModel.Instance.SpawnedApplicationStateManagerSpwanerReference.Spawn();
                SpawnerModel.Instance.SpawnedDebugLogManagerSpawnerReference.Spawn();
                SpawnerModel.Instance.SpawnedBarMagnetSyncPoint01SpawnerReference.Spawn();
                BarMagnetModel.Instance.IsDrawing = false;
                CompassesModel.Instance.IsShown = false;
                break;
            case ApplicationStateModel.State.DrawLines:
                BarMagnetModel.Instance.IsDrawing = true;
                CompassesModel.Instance.IsShown = false;
                break;
            case ApplicationStateModel.State.ShowCompass:
                BarMagnetModel.Instance.IsDrawing = false;
                CompassesModel.Instance.IsShown = true;
                break;
            default:
                break;
        }

        applicationDebugLogWriter.AnchorDebugText.text += "\nChanged to: " + applicationStateCurrent.ToString();
    }

    private bool SpawnedApplicationManagerExists()
    {
        GameObject spawnedApplicationManager = null;
        string objectTag = "SpawnedApplicationManager";
        spawnedApplicationManager = GameObject.FindWithTag(objectTag);
        return (spawnedApplicationManager != null);
    }
}

using HoloToolkit.Unity;
using UnityEngine;

public class DebugLogVisiblityPresenter : Singleton<DebugLogVisiblityPresenter> {

    private bool isShownCurrent = false;
    private bool isShownOld = false;

	// Update is called once per frame
	void Update () {
        // Observer
        isShownCurrent = DebugLogModel.Instance.IsShown;
        if (isShownCurrent != isShownOld)
        {
            Present();

            // --- 終了処理 ---
            // 表示状態を保存する
            isShownOld = isShownCurrent;
        }
	}

    private void Present()
    {
        switch (isShownCurrent)
        {
            case true:
                DebugLogModel.Instance.ApplicationDebugLogReference.SetActive(true);
                DebugLogModel.Instance.SharingDebugLogReference.SetActive(true);
                break;
            case false:
                DebugLogModel.Instance.ApplicationDebugLogReference.SetActive(false);
                DebugLogModel.Instance.SharingDebugLogReference.SetActive(false);
                break;
            default:
                break;
        }
    }

    private bool SpawnedApplicationManagerExists()
    {
        GameObject spawnedApplicationManager = null;
        string objectTag = "SpawnedApplicationManager";
        spawnedApplicationManager = GameObject.FindWithTag(objectTag);
        return (spawnedApplicationManager != null);
    }
}

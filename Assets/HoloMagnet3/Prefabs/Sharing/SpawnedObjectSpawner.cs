using HoloToolkit.Sharing.Spawning;
using UnityEngine;
using HoloToolkit.Sharing;

/// <summary>
/// タップするとSpawnedApplicationManagerをSpawnする
/// </summary>
public class SpawnedObjectSpawner : MonoBehaviour
{
    protected string objectNameToSpawn = "";

    protected PrefabSpawnManager prefabSpawnManager = null;
    protected GameObject sharingObject = null;
    protected SyncSpawnedObject syncSpawnedObject = null;

    // Todo: ApplicationDebugLogWriter
    //protected ApplicationDebugLogWriter applicationDebugLogWriter = null;

    public void Start()
    {
        // Todo: ApplicationDebugLogWriter
        //applicationDebugLogWriter = GetComponent<ApplicationDebugLogWriter>();

        sharingObject = SharingObjectReferenceEnabler.Instance.SharingObjectReference;
    }

    /// <summary>
    /// SpawnedApplicationManagerをSpawnする
    /// </summary>
    /// <param name="eventData"></param>

    public void Spawn()
    {
        // シェアリング中でなければreturnする
        if (!SharingStage.Instance.IsConnected) return;

        prefabSpawnManager = PrefabSpawnManagerReferenceEnabler.Instance.Reference;
        if (prefabSpawnManager == null)
        {
            Debug.Log("spawnManager is not found. Quit spawning " + objectNameToSpawn);
            return;
        }

        // Todo: ApplicationDebugLogWriter
        //applicationDebugLogWriter.AnchorDebugText.text += "\n" + objectNameToSpawn + " is spawned.";
    }
}

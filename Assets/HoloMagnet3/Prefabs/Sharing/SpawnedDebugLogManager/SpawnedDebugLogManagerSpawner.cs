using FeelPhysics.HoloMagnet36;
using UnityEngine;

/// <summary>
/// タップするとSpawnedDebugLogManagerをSpawnする
/// </summary>
public class SpawnedDebugLogManagerSpawner : SpawnedObjectSpawner
{
    private new SyncSpawnedDebugLogManager syncSpawnedObject = null;

    private new void Start()
    {
        base.Start();
        objectNameToSpawn = "SpawnedDebugLogManager";
        syncSpawnedObject = new SyncSpawnedDebugLogManager();
        SpawnerModel.Instance.SpawnedDebugLogManagerSpawnerReference = this;
    }

    public new void Spawn()
    {
        base.Spawn();
        prefabSpawnManager.Spawn(
            syncSpawnedObject, Vector3.zero, Quaternion.identity, null, objectNameToSpawn, true);
    }
}

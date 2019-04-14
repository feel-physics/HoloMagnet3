using FeelPhysics.HoloMagnet36;
using UnityEngine;

/// <summary>
/// タップするとSpawnedApplicationStateManagerをSpawnする
/// </summary>
public class SpawnedBarMagnetSyncPoint01Spawner : SpawnedObjectSpawner
{
    private new SyncSpawnedBarMagnetSyncPoint syncSpawnedObject = null;

    private new void Start()
    {
        base.Start();
        objectNameToSpawn = "SpawnedBarMagnetSyncPoint01";
        syncSpawnedObject = new SyncSpawnedBarMagnetSyncPoint();
        SpawnerModel.Instance.SpawnedBarMagnetSyncPoint01SpawnerReference = this;
    }

    public new void Spawn()
    {
        base.Spawn();
        prefabSpawnManager.Spawn(
            syncSpawnedObject, Vector3.zero, Quaternion.identity, null, objectNameToSpawn, true);
    }
}

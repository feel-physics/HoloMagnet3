using FeelPhysics.HoloMagnet36;
using UnityEngine;

/// <summary>
/// タップするとSpawnedApplicationStateManagerをSpawnする
/// </summary>
public class SpawnedApplicationStateManagerSpwaner : SpawnedObjectSpawner
{
    private new SyncSpawnedApplicationStateManager syncSpawnedObject = null;

    private new void Start()
    {
        base.Start();
        objectNameToSpawn = "SpawnedApplicationStateManager";
        syncSpawnedObject = new SyncSpawnedApplicationStateManager();
        SpawnerModel.Instance.SpawnedApplicationStateManagerSpwanerReference = this;
    }

    public new void Spawn()
    {
        base.Spawn();
        prefabSpawnManager.Spawn(
            syncSpawnedObject, Vector3.zero, Quaternion.identity, null, objectNameToSpawn, true);
    }
}

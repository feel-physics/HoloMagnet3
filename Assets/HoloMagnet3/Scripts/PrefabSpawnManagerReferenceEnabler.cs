using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Unity;

public class PrefabSpawnManagerReferenceEnabler : Singleton<PrefabSpawnManagerReferenceEnabler>
{
    public PrefabSpawnManager Reference = null;

    void Start()
    {
        // Todo: GetComponentを使っている。Start()だからいいか？
        Reference = gameObject.GetComponent<PrefabSpawnManager>();
    }
}

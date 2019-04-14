using HoloToolkit.Unity;
using UnityEngine;

public class CompassesCreator : Singleton<CompassesCreator> {

    /// <summary>
    /// CompassPlacer3Dは自身で自分を消去する
    /// </summary>
    public void Create()
    {
        GameObject compassPlacer3D =
            (GameObject)Resources.Load("CompassPlacer/CompassPlacer3D");
        if (compassPlacer3D == null) Debug.LogError("compassPlacer3D is null");
        Instantiate(compassPlacer3D);
    }
}

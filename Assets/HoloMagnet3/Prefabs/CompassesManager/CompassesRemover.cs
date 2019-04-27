using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

public class CompassesRemover : Singleton<CompassesRemover> {

    /// <summary>
    /// 方位磁針をすべて削除する
    /// </summary>
    public void Remove()
    {
        List<GameObject> compasses = CompassesModel.Instance.CompassesReference;
        foreach (GameObject compass in compasses)
        {
            Destroy(compass);
        }

        CompassesModel.Instance.ClearAllReference();
    }
}

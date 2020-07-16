using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompassesModel))]
public class CompassesRemover : MonoBehaviour
{

    /// <summary>
    /// 方位磁針をすべて削除する
    /// </summary>
    public void Remove()
    {
        var compassesModel = GetComponent<CompassesModel>();
        List<GameObject> compasses = compassesModel.CompassesReference;
        foreach (GameObject compass in compasses)
        {
            Destroy(compass);
        }

        compassesModel.ClearAllReference();
    }
}

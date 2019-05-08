using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

public class CompassesModel : Singleton<CompassesModel>
{
    public List<GameObject> CompassesReference;
    public List<CompassManagedlyUpdater> CompassesReferenceForManagedlyUpdate;

    /// <summary>
    /// コンパスたちの親Transform。値はCompassCreatorから設定される
    /// </summary>
    public Transform ParentTransform;

    /// <summary>
    ///コンパスの間隔。値はCompassCreatorから設定される
    /// </summary>
    public float pitch = 0.07f;

    public void ClearAllReference()
    {
        CompassesReference.Clear();
        CompassesReferenceForManagedlyUpdate.Clear();
    }
}
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

public class CompassesModel : Singleton<CompassesModel>
{
    public List<GameObject> CompassesReference;
    public List<CompassManagedlyUpdater> CompassesReferenceForManagedlyUpdate;

    public void ClearAllReference()
    {
        CompassesReference.Clear();
        CompassesReferenceForManagedlyUpdate.Clear();
    }
}
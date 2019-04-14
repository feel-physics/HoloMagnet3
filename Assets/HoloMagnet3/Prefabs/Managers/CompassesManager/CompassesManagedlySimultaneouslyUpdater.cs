using System.Collections.Generic;
using UnityEngine;

public class CompassesManagedlySimultaneouslyUpdater : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        // Observer
        var compasses = CompassesModel.Instance.CompassesReferenceForManagedlyUpdate;

        // Presenter
        if (compasses == null) return;
        ManagedlyUpdate(compasses);
    }

    void ManagedlyUpdate(List<CompassManagedlyUpdater> compasses)
    {
        foreach (CompassManagedlyUpdater compass in compasses)
        {
            compass.ManagedlyUpdate();
        }
    }
}

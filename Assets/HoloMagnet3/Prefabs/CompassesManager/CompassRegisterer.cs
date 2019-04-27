using UnityEngine;

public class CompassRegisterer : MonoBehaviour {
    private void Start()
    {
        Register();
    }

    private void Register()
    {
        // Compasses Modelに自分を登録する
        CompassesModel.Instance.CompassesReference.Add(gameObject);
        CompassesModel.Instance.CompassesReferenceForManagedlyUpdate.
            Add(gameObject.GetComponent<CompassManagedlyUpdater>());
    }
}

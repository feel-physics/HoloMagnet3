using UnityEngine;

public class CompassRegisterer : MonoBehaviour {


    public void Register(CompassesModel compassesModel)
    {
        //Debug.Log("RegisterRegisterRegisterRegisterRegisterRegisterRegisterRegisterRegisterRegisterRegisterRegisterRegisterRegister");
        // Compasses Modelに自分を登録する
        compassesModel.CompassesReference.Add(gameObject);
        compassesModel.CompassesReferenceForManagedlyUpdate.
            Add(gameObject.GetComponent<CompassManagedlyUpdater>());
    }
}

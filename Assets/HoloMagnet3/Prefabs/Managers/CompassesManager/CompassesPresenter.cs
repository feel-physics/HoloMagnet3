using UnityEngine;

public class CompassesPresenter : MonoBehaviour {

    private bool isShownCurrent = false;
    private bool isShownOld = false;

    // Use this for initialization
    void Update () {
        isShownCurrent = CompassesModel.Instance.IsShown;
        if (isShownCurrent != isShownOld)
        {
            Present(isShownCurrent);
        }
        isShownOld = isShownCurrent;
    }

    void Present(bool isShownCurrent)
    {
        switch (isShownCurrent)
        {
            case true:
                CompassesCreator.Instance.Create();
                break;
            case false:
                CompassesRemover.Instance.Remove();
                break;
            default:
                CompassesRemover.Instance.Remove();
                break;
        }
    }
}

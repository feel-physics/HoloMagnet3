using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class BarMagnetInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<BarMagnetModel>().NorthPoleReference =
            transform.Find("North Body/North Pole").gameObject;
        gameObject.GetComponent<BarMagnetModel>().SouthPoleReference =
            transform.Find("South Body/South Pole").gameObject;
        gameObject.GetComponent<BarMagnetModel>().MagneticForceLineReference = 
            transform.Find("MagneticForceLine").gameObject;
        //BarMagnetModel.Instance.NorthPoleReference = transform.Find("North Body/North Pole").gameObject;
        //BarMagnetModel.Instance.SouthPoleReference = transform.Find("South Body/South Pole").gameObject;
        //BarMagnetModel.Instance.MagneticForceLineReference = transform.Find("MagneticForceLine").gameObject;

        //準備出来たらGlobalListenerに追加
        if (gameObject.GetComponent<SetGlobalListener>() == null)
            gameObject.AddComponent<SetGlobalListener>();

    }
}

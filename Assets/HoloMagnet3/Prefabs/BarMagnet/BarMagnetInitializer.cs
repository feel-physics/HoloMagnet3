using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class BarMagnetInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<BarMagnetModel>().NorthPoleReference =
            transform.Find("North Body/North Pole").gameObject;
        gameObject.GetComponent<BarMagnetModel>().SouthPoleReference =
            transform.Find("South Body/South Pole").gameObject;


        //準備出来たらGlobalListenerに追加
        if (gameObject.GetComponent<SetGlobalListener>() == null)
            gameObject.AddComponent<SetGlobalListener>();

    }
}

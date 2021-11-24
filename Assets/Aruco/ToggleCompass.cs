using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCompass : MonoBehaviour {
	// Start is called before the first frame update
	[SerializeField] private bool isCompassDisplay;
	[SerializeField] private GameObject compass;
	private CompassesCreator creator;
	private CompassesRemover remover;
	void Start() {
		creator = compass.GetComponent<CompassesCreator>();
		remover = compass.GetComponent<CompassesRemover>();
	}

	// Update is called once per frame
	void Update() { }
	
	//コンパス表示ON/OFFを切り替える
	public void ToggleCompassDisplay() {
		if (creator == null || remover == null) return;
		isCompassDisplay = !isCompassDisplay;
		if (isCompassDisplay) {
			creator.RebuildCompass();
		}
		else {
			remover.Remove();			
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transformをカメラの子要素にする
/// </summary>
public class AttachToCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.parent = Camera.main.transform;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;


    }

}

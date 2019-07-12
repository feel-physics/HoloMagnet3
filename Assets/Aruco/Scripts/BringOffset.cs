using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringOffset : MonoBehaviour {

    [SerializeField]
    Transform target;


	// Update is called once per frame
	void Update () {
        if (transform.hasChanged && target != null)
        {
            target.SetPositionAndRotation(transform.position, transform.rotation);
        }
	}
}

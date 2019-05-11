using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMagnetRestrictMovement : MonoBehaviour {

    public bool Is2D = false;

	// Use this for initialization
	void Start () {
        if (MySceneManager.Instance.MyScene == MySceneManager.MySceneEnum.Compasses_2D)
        {
            Is2D = true;
        }
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!Is2D) return;  // 2次元のシーンでなければreturn  Todo: 必要か？
        Vector3 pos = transform.position;
        pos.z = 2;
        transform.position = pos;
    }
}

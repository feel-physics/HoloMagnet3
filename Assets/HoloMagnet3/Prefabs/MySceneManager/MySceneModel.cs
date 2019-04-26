using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MySceneModel : MonoBehaviour {

    // シーンのリストをenumで作る
    public enum MyScene { Introduction, Compass_One, Compasses_2D, Compasses_3D }
    public MyScene scene;

    // シーン名とenumのシーンとを対応させる
    Dictionary<string, MyScene> sceneDic = new Dictionary<string, MyScene>() {
        {"Introduction",    MyScene.Introduction },
        {"Compass_One",     MyScene.Compass_One },
        {"Compasses_2D",    MyScene.Compasses_2D },
        {"Compasses_3D",    MyScene.Compasses_3D }
    };

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

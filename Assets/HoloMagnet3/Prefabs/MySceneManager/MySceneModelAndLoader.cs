using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneModelAndLoader : MonoBehaviour {

    // シーンのリストをenumで作る
    public enum MySceneEnum { Introduction, Compass_One, Compasses_2D, Compasses_3D }
    public MySceneEnum MyScene;

    // シーン名とenumのシーンとを対応させる
    public Dictionary<string, MySceneEnum> MySceneDic = new Dictionary<string, MySceneEnum>() {
        {"Introduction",    MySceneEnum.Introduction },
        {"Compass_One",     MySceneEnum.Compass_One },
        {"Compasses_2D",    MySceneEnum.Compasses_2D },
        {"Compasses_3D",    MySceneEnum.Compasses_3D }
    };

    // ApplicationStateを操作する準備
    private MySceneEnum sceneOld =     MySceneEnum.Introduction;
    private MySceneEnum sceneCurrent = MySceneEnum.Introduction;

    void Start () {
        // 初期化
        MyScene = MySceneEnum.Introduction;
	}
	
	void Update () {
        // 現在のSceneを取得する
        sceneCurrent = MyScene;

        // Sceneが変わっていたら該当処理を行う
        if (sceneCurrent != sceneOld)
        {
            Load(sceneCurrent);
        }

        // --- 終了処理 ---
        // Applicationの状態を保存する
        sceneOld = sceneCurrent;
    }

    // enumのシーンで指定したシーンをロードする
    public void Load(MySceneEnum scene)
    {
        SceneManager.LoadScene(MySceneDic.FirstOrDefault(x => x.Value == scene).Key);
    }
}

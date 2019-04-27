using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class MySceneManager : Singleton<MySceneManager> {

    public int sceneId = 0;

    void Start()
    {
        // 初期化
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Introduction":
                sceneId = 0;
                break;
            case "Compass_One":
                sceneId = 1;
                break;
            case "Compasses_2D":
                sceneId = 2;
                break;
            case "Compasses_3D":
                sceneId = 3;
                break;
            default:
                break;
        }
    }

    public void LoadNextScene()
    {
        string sceneName;

        if (sceneId == 3)
        {
            sceneId = 0;
        }
        else
        {
            sceneId++;
        }

        switch (sceneId)
        {
            case 0:
                sceneName = "Introduction";
                break;
            case 1:
                sceneName = "Compass_One";
                break;
            case 2:
                sceneName = "Compasses_2D";
                break;
            case 3:
                sceneName = "Compasses_3D";
                break;
            default:
                throw new System.Exception("sceneId is invalid");
        }
        SceneManager.LoadScene(sceneName);
    }


    /*
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
    public void Load(int sceneId)
{
    // 方位磁針をすべて削除する
    CompassesRemover.Instance.Remove();
    // シーンをロードする
    //SceneManager.LoadScene(MySceneDic.FirstOrDefault(x => x.Value == scene).Key);
    SceneManager.LoadScene(sceneNames[sceneId]);
}

public void LoadNextScene()
{
    /*
    switch (MyScene)
    {
        case MySceneEnum.Introduction:
            MyScene = MySceneEnum.Compass_One;
            break;
        case MySceneEnum.Compass_One:
            MyScene = MySceneEnum.Compasses_2D;
            break;
        case MySceneEnum.Compasses_2D:
            MyScene = MySceneEnum.Compasses_3D;
            break;
        case MySceneEnum.Compasses_3D:
            MyScene = MySceneEnum.Introduction;
            break;
        default:
            break;
    };
    Load(MyScene);

    sceneId++;
    Load(sceneId);
}
}
    */
}
using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class MySceneManager : Singleton<MySceneManager> {

    public enum MySceneEnum { Introduction, Compass_One, Compasses_2D, Compasses_3D };
    public MySceneEnum MyScene;

    // シーン名とenumのシーンとを対応させる
    private Dictionary<string, MySceneEnum> sceneDic = new Dictionary<string, MySceneEnum>() {
        {"Introduction",    MySceneEnum.Introduction },
        {"Compass_One",     MySceneEnum.Compass_One },
        {"Compasses_2D",    MySceneEnum.Compasses_2D },
        {"Compasses_3D",    MySceneEnum.Compasses_3D }
    };

    protected override void Awake()
    {
        // Singletonのオーバーライド
        base.Awake();

        // シーンAwake時に、MySceneに現在のシーンを代入する
        string sceneName = SceneManager.GetActiveScene().name;
        MyScene = sceneDic[sceneName];

        // オブジェクトを初期化する
        ObjectsInitializer.Instance.Initialize();

#if false
        // 初期化  Todo: 掃除する
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Introduction":
                SceneId = 0;
                break;
            case "Compass_One":
                SceneId = 1;
                break;
            case "Compasses_2D":
                SceneId = 2;
                break;
            case "Compasses_3D":
                SceneId = 3;
                break;
            default:
                break;
        }
#endif
    }

    public void LoadNextScene()
    {
        MySceneEnum nextScene;

        switch (MyScene)
        {
            case MySceneEnum.Introduction:
                nextScene = MySceneEnum.Compass_One;
                break;
            case MySceneEnum.Compass_One:
                nextScene = MySceneEnum.Compasses_2D;
                break;
            case MySceneEnum.Compasses_2D:
                nextScene = MySceneEnum.Compasses_3D;
                break;
            case MySceneEnum.Compasses_3D:
                nextScene = MySceneEnum.Introduction;
                break;
            default:
                throw new System.Exception("Invarid MyScene");
        }

        MyLoadScene(nextScene);
#if false
        string sceneName;

        if (SceneId == 3)
        {
            SceneId = 0;
        }
        else
        {
            SceneId++;
        }

        switch (MyScene)
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
#endif
    }

    // enumのシーンで指定したシーンをロードする
    public void MyLoadScene(MySceneEnum scene)
    {
        SceneManager.LoadScene(sceneDic.FirstOrDefault(x => x.Value == scene).Key);
    }
}
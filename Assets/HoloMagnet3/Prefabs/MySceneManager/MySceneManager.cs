using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class MySceneManager : Singleton<MySceneManager> {

    public int SceneId = 0;

    protected override void Awake()
    {
        base.Awake();

        // 初期化
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
    }

    public void LoadNextScene()
    {
        string sceneName;

        if (SceneId == 3)
        {
            SceneId = 0;
        }
        else
        {
            SceneId++;
        }

        switch (SceneId)
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
}
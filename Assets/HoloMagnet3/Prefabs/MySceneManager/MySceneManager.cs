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
}
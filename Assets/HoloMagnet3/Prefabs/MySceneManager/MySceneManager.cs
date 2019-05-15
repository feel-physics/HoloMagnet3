using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : Singleton<MySceneManager> {

    public enum MySceneEnum { Introduction, Compass_One, Compasses_2D, Compasses_3D };
    public MySceneEnum MyScene;

    [SerializeField]
    private AudioClip acLoadNextScene;

    private AudioSource audioSource;

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
    }

    public void LoadNextScene()
    {
        MySceneEnum nextScene;

        audioSource = GetComponents<AudioSource>()[0];
        audioSource.clip = acLoadNextScene;
        audioSource.loop = false;

        switch (MyScene)
        {
            case MySceneEnum.Introduction:
                nextScene = MySceneEnum.Compass_One;
                // Todo: 後でシーン遷移の音をシーン毎に変える
                audioSource.pitch = 1.0f;
                break;
            case MySceneEnum.Compass_One:
                nextScene = MySceneEnum.Compasses_2D;
                audioSource.pitch = 1.5f;
                break;
            case MySceneEnum.Compasses_2D:
                nextScene = MySceneEnum.Compasses_3D;
                audioSource.pitch = 2.0f;
                break;
            case MySceneEnum.Compasses_3D:
                nextScene = MySceneEnum.Introduction;
                audioSource.pitch = 2.5f;
                break;
            default:
                throw new System.Exception("Invarid MyScene");
        }

        MyLoadScene(nextScene);

        // 音を鳴らす
        audioSource.Play();
    }

    // enumのシーンで指定したシーンをロードする
    // 使い回すためメソッドとして切り出している
    public void MyLoadScene(MySceneEnum scene)
    {
        SceneManager.LoadScene(sceneDic.FirstOrDefault(x => x.Value == scene).Key);
    }
}
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Sharing;
using FeelPhysics.HoloMagnet36;

// グローバル定数
public static class GlobalVar
{
    public const Boolean shouldDebugLog = true;
    public const Boolean shouldDebugLogInScene = true;
}

public class MyHelper : MonoBehaviour
{
    // シーンオブジェクト
    public static void DebugLogInScene(String logObjectName, String message)
    {
        if (GlobalVar.shouldDebugLogInScene)
        {
            TextMesh tm = GameObject.Find(logObjectName).GetComponent<TextMesh>();
            tm.text = message;
        }
    }

    // シーンのリストをenumで作る
    public enum MyScene { Operation, Point, Scene2D, Scene3D }
    public static MyScene scene;

    // シーン名とenumのシーンとを対応させる
    static Dictionary<string, MyScene> sceneDic = new Dictionary<string, MyScene>() {
        {"Operation", MyScene.Operation },
        {"Point",     MyScene.Point },
        {"Scene2D",   MyScene.Scene2D },
        {"Scene3D",   MyScene.Scene3D }
    };

    // 現在のシーンを取得する
    public static MyScene MyGetScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        scene = sceneDic[sceneName];
        return scene;
    }

    // enumのシーンで指定したシーンをロードする
    public static void MyLoadScene(MyScene scene)
    {
        SceneManager.LoadScene(sceneDic.FirstOrDefault(x => x.Value == scene).Key);
    }

    public class MyMonoBehaviour : MonoBehaviour
    {
        public void CallStartCoroutine(IEnumerator iEnumerator)
        {
            StartCoroutine(iEnumerator); //ここで実際にMonoBehaviour.StartCoroutine()を呼ぶ
        }
    }

    // AsyncAwaitでできる
    private static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    // AsyncAwaitでできる
    public static void MyDelayMethod(MonoBehaviour i_behaviour, float waitTime, Action action)
    {
        i_behaviour.StartCoroutine(DelayMethod(waitTime, action));
    }

    // ※ILoggableEvery10Seconds
    public static void DebugLogEvery10Seconds(string message, ref bool hasLogged)
    {
        if (DateTime.Now.Second % 10 == 0)
        {
            if (!hasLogged)
            {
                Debug.Log(message);
                hasLogged = true;
            }
        }
        else
        {
            hasLogged = false;
        }
    }

    /*
    public static void SetSyncModelIsMagneticForceLines(bool shouldDraw)
    {
        // Spawned Application ManagerのSharingしている変数を変更する
        if (SharingStage.Instance.IsConnected)
        {
            var sam = GameObject.FindWithTag("SpawnedApplicationManager");
            var ssam = (SyncSpawnedApplicationManager)sam.
                GetComponent<DefaultSyncModelAccessor>().SyncModel;
            ssam.isMagneticForceLines.Value = shouldDraw;
        }
    }

    public static void ToggleDrawOrDeleteMagneticForceLines()
    {
        Debug.Log("MyHelper.DrawOrDeleteMagneticForceLines() script is fired");

        // BarMagnetを探す
        string barMagnetTag = "BarMagnet01";
        var barmagnetObject = GameObject.FindWithTag(barMagnetTag);
        if (barmagnetObject == null)
        {
            Debug.Log(barMagnetTag + " is not found");
        }
        else
        {
            // 磁力線を描画しているか否かを取得し、ログ出力する
            var state = barmagnetObject.GetComponent<MagneticForceLinesManager>().state;
            Debug.Log(barMagnetTag + " state: " + state.ToString());

            // 磁力線を描画していれば消す
            if (state == MagneticForceLinesManager.State.Drawing3D)
            {
                Debug.Log(barMagnetTag + " delete magnetic force lines.");
                barmagnetObject.GetComponent<MagneticForceLinesManager>().state =
                    MagneticForceLinesManager.State.NotDrawing;
                Debug.Log(barMagnetTag + " is set as NotDrawing");
                barmagnetObject.GetComponent<MagneticForceLinesManager>().DeleteLines();

                MyHelper.SetSyncModelIsMagneticForceLines(false);
            }
            // 磁力線を描画していなければ描画する
            else
            {
                // Spawned Application ManagerのSharingしている変数を変更する
                Debug.Log(barMagnetTag + " doesn't draw magnetic force lines. Will draw them.");
                barmagnetObject.GetComponent<MagneticForceLinesManager>().state =
                    MagneticForceLinesManager.State.Drawing3D;
                Debug.Log(barMagnetTag + " is set as Drawing3D");
                barmagnetObject.GetComponent<MagneticForceLinesManager>().DrawLines3D();

                MyHelper.SetSyncModelIsMagneticForceLines(true);
            }
        }
    }
    */
}
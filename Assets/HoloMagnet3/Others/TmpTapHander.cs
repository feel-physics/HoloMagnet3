using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TmpTapHander : MonoBehaviour, IInputClickHandler
{
    public void Start()
    {
        // FallBackEventHandlerにする
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    /*
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // ローカルApplicationManagerの状態を1つ進める
        ApplicationStateModel.Instance.Shift();
    }
    */

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Tapped");
        MySceneManager.Instance.LoadNextScene();
    }
}

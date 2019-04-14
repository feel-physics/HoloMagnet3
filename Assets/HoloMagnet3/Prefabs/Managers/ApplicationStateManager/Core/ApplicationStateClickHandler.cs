using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ApplicationStateClickHandler : MonoBehaviour, IInputClickHandler
{
    public void Start()
    {
        // FallBackEventHandlerにする
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        // ローカルApplicationManagerの状態を1つ進める
        ApplicationStateModel.Instance.Shift();
    }
}
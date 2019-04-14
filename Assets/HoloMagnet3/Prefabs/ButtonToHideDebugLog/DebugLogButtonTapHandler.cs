using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace ButtonToHideDebugLog
{
    public class DebugLogButtonTapHandler : MonoBehaviour, IInputClickHandler
    {
        public void OnInputClicked(InputClickedEventData eventData)
        {
            DebugLogModel.Instance.IsShown = !DebugLogModel.Instance.IsShown;
        }
    }
}
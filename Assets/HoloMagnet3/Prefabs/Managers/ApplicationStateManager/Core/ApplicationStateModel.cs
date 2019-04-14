using HoloToolkit.Sharing;
using HoloToolkit.Unity;

public class ApplicationStateModel : Singleton<ApplicationStateModel> {
    public enum State { ShowNothing, SharingInitializing, DrawLines, ShowCompass};
    public State state = State.ShowNothing;
    public bool SharingIsInitialized = false; 

    /// <summary>
    /// ローカルApplicationManagerの状態を1つ進める
    /// </summary>
    public void Shift()
    {
        switch (state)
        {
            case State.ShowNothing:
                if (SharingStage.Instance.IsConnected && !SharingIsInitialized)
                {
                    state = State.SharingInitializing;
                    SharingIsInitialized = true;
                }
                else
                {
                    state = State.DrawLines;
                }
                break;
            case State.SharingInitializing:
                state = State.DrawLines;
                break;
            case State.DrawLines:
                state = State.ShowCompass;
                break;
            case State.ShowCompass:
                state = State.ShowNothing;
                break;
            default:
                break;
        }
    }
}

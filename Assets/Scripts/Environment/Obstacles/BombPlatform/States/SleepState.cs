public class SleepState : BombPlatformState
{
    public override void EnterState(BombPlatformFSM bombPlatform)
    {
        bombPlatform.ChangeColor(bombPlatform.StartColor);
    }

    public override async void OnTriggerStay(BombPlatformFSM bombPlatform)
    {
        await bombPlatform.TransitionToStateAsync(bombPlatform.AwakeningState, 0.0f);
    }
}

public class ResetState : BombPlatformState
{
    public override async void EnterState(BombPlatformFSM bombPlatform)
    {
        await bombPlatform.TransitionToStateAsync(bombPlatform.SleepState, bombPlatform.ResetDuration);
    }

    public override void OnTriggerStay(BombPlatformFSM bombPlatform)
    {
    }
}

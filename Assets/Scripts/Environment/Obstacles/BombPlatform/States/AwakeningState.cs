public class AwakeningState : BombPlatformState
{
    public override async void EnterState(BombPlatformFSM bombPlatform)
    {
        bombPlatform.ChangeColor(bombPlatform.AwakeningColor);

        await bombPlatform.TransitionToStateAsync(bombPlatform.ExplosionState, bombPlatform.AwakeningDuration);
    }

    public override void OnTriggerStay(BombPlatformFSM bombPlatform)
    {
    }
}

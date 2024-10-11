public class ExplosionState : BombPlatformState
{
    public override async void EnterState(BombPlatformFSM bombPlatform)
    {
        bombPlatform.ChangeColor(bombPlatform.DetonateColor);

        bombPlatform.Detonate();

        await bombPlatform.TransitionToStateAsync(bombPlatform.ResetState, 0.0f);
    }

    public override void OnTriggerStay(BombPlatformFSM bombPlatform)
    {
    }
}

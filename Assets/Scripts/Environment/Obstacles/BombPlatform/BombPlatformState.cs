public abstract class BombPlatformState
{
    public abstract void EnterState(BombPlatformFSM bombPlatform);
    public abstract void OnTriggerStay(BombPlatformFSM bombPlatform);
}

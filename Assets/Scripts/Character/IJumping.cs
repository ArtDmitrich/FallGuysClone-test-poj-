public interface IJumping
{
    public bool IsGrounded { get; }
    public float VelocityY { get; }
    public void Jump();
}

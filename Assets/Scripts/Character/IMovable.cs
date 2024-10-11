using UnityEngine;

public interface IMovable
{
    public void StartMovement(Vector2 direction);
    public void StopMovement();
    public void Jump();
}

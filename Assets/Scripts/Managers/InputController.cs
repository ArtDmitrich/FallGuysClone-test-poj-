using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public UnityAction<Vector2> PlayerMovementStarted;
    public UnityAction PlayerMovementStoped;
    public UnityAction PlayerJumped;

    private InputActions Input { get { return _input = _input ?? new InputActions(); } }
    private InputActions _input;

    private void Movement_started(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        var direction = ctx.ReadValue<Vector2>();
        PlayerMovementStarted?.Invoke(direction);
    }

    private void Movement_canceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        PlayerMovementStoped?.Invoke();
    }
    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        PlayerJumped?.Invoke();
    }

    private void OnEnable()
    {
        Input.Enable();

        Input.Player.Movement.performed += Movement_started;
        Input.Player.Movement.canceled += Movement_canceled;
        Input.Player.Jump.performed += Jump_performed;
    }


    private void OnDisable()
    {
        Input.Disable();

        Input.Player.Movement.performed -= Movement_started;
        Input.Player.Movement.canceled -= Movement_canceled;
        Input.Player.Jump.performed -= Jump_performed;
    }
}

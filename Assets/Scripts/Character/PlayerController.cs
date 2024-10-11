using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour, ITakingDamage
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _groundCheckDistance;
    private IMovable Movement { get { return _movement = _movement ?? GetComponent<IMovable>(); } }
    private IMovable _movement;

    private IJumping Jumping { get { return _jumping = _jumping ?? GetComponent<IJumping>(); } }
    private IJumping _jumping;

    private IRotatable Rotator { get { return _rotator = _rotator ?? GetComponentInChildren<IRotatable>(); } }
    private IRotatable _rotator;

    private Animator Animator { get { return _animator = _animator ?? GetComponentInChildren<Animator>(); } }
    private Animator _animator;

    private InputController _input;
    private GameplayResources _resources;

    [Inject]
    private void Construct(InputController input, GameplayResources gameplayResources)
    {
        _input = input;
        _resources = gameplayResources;
    }

    public void TakeDamage(int damage)
    {
        _resources.ChangePlayerHealth(-damage);
    }

    private void StartMovement(Vector2 direction)
    {
        Vector3 cameraForward = _camera.forward;
        cameraForward.Normalize();

        Vector3 cameraRight = _camera.right;
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraRight * direction.x) + (cameraForward * direction.y);
        var result = new Vector2(moveDirection.x, moveDirection.z);

        Movement?.StartMovement(result);
        Rotator.SetDirectionToRotate(result);

        Animator.SetFloat("Speed", 1.0f);
    }

    private void StopMovement()
    {
        Movement?.StopMovement();

        Animator.SetFloat("Speed", 0.1f);
    }

    private void Jump()
    {
        Jumping.Jump();

        Animator.SetTrigger("Jump");
    }

    private void Update()
    {
        var velocityY = Jumping.VelocityY;
        var isGrounded = Jumping.IsGrounded;

        Animator.SetFloat("VelocityY", velocityY);
        Animator.SetBool("Grounded", isGrounded);

        if(velocityY != 0.0f && CheckLanding())
        {
            Animator.SetTrigger("Landing");
        }
    }

    private bool CheckLanding()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundCheckDistance))
        {
            return true;
        }

        return false;
    }

    protected void OnEnable()
    {
        _input.PlayerMovementStarted += StartMovement;
        _input.PlayerMovementStoped += StopMovement;
        _input.PlayerJumped += Jump;
    }

    protected void OnDisable()
    {
        _input.PlayerMovementStarted -= StartMovement;
        _input.PlayerMovementStoped -= StopMovement;
        _input.PlayerJumped -= Jump;
    }
}

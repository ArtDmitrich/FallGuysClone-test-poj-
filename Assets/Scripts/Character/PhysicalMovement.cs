using System;
using UnityEngine;

public class PhysicalMovement : MonoBehaviour, IMovable, IJumping
{
    public bool IsGrounded { get { return _isGrounded; } }
    public float VelocityY { get { return Rb.velocity.y; } }

    [SerializeField] private float _maxMovementSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _jumpForce;

    private Rigidbody Rb { get { return _rb = _rb ?? GetComponent<Rigidbody>(); } }
    private Rigidbody _rb;

    private bool _isMoving;
    private Vector3 _movementVelocity;
    private Vector2 _direction;
    private bool _isGrounded;

    public void StartMovement(Vector2 direction)
    {
        _isMoving = true;
        _direction = direction;
    }

    public void StopMovement()
    {
        _isMoving = false;
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            Rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            _movementVelocity = Vector3.Lerp(_movementVelocity, new Vector3(_direction.x, 0.0f, _direction.y).normalized * _maxMovementSpeed, _acceleration);
        }
        else
        {
            _movementVelocity = Vector3.Lerp(_movementVelocity, Vector3.zero, _deceleration);
        }

        //Rb.velocity = transform.TransformVector(new Vector3(_movementVelocity.x, Rb.velocity.y, _movementVelocity.z));
        Rb.velocity = new Vector3(_movementVelocity.x, Rb.velocity.y, _movementVelocity.z);

        //Rb.velocity = transform.InverseTransformVector(new Vector3(_movementVelocity.x, Rb.velocity.y, _movementVelocity.z));
    }

    private void OnCollisionStay(Collision collision)
    {
        var normal = collision.contacts[0].normal;

        var dot = Vector3.Dot(normal, Vector3.up);

        if (dot > 0.4f)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
    }

}

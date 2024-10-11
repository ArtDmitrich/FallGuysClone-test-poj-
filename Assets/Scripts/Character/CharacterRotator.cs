using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotator : MonoBehaviour, IRotatable
{
    [SerializeField] private float _rotationSpeed = 10f;

    private Vector3 _movementDirection;

    public void SetDirectionToRotate(Vector3 direction)
    {
        _movementDirection = new Vector3(direction.x, 0.0f, direction.y);
    }

    private void Update()
    {
        RotateTowardsMovement();
    }

    private void RotateTowardsMovement()
    {
        if (_movementDirection.magnitude > 0.1f)
        {
            var targetRotation = Quaternion.LookRotation(_movementDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }
}

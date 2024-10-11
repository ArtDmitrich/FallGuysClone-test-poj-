using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WindPlatform : MonoBehaviour
{
    [SerializeField] private float _windDuration;
    [SerializeField] private float _windPower;
    [SerializeField] private Transform _arrow;

    private List<Rigidbody> _targetRigidbodies = new List<Rigidbody>();
    private Vector3 _windDirection;

    private async void Start()
    {
        await SetNewWindDirection(_windDuration);
    }

    private void ChangeWindDirection()
    {
        var x = UnityEngine.Random.Range(-1.0f, 1.0f);
        var z = UnityEngine.Random.Range(-1.0f, 1.0f);

        _windDirection = new Vector3(x, 0.0f, z).normalized;

        _arrow.LookAt(_arrow.position + _windDirection);
    }

    private async UniTask SetNewWindDirection(float duration)
    {
        if (duration <= 0.0f) 
        {
            return;
        }

        ChangeWindDirection();

        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        await SetNewWindDirection(_windDuration);
    }

    private void FixedUpdate()
    {
        if (_targetRigidbodies.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _targetRigidbodies.Count; i++)
        {
            _targetRigidbodies[i].AddForce(_windDirection * _windPower, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            _targetRigidbodies.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb) && _targetRigidbodies.Contains(rb))
        {
            _targetRigidbodies.Remove(rb);
        }
    }
}

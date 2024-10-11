using DG.Tweening;
using System;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour, IRestartable
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _durationOfDisapperance;

    [SerializeField] private ParticleSystem _disableEffect;

    private Platform Platform { get { return _platform = _platform ?? GetComponentInChildren<Platform>(); } }
    private Platform _platform;

    private bool _isAlive = true;
    private Color _startColor;

    public void Restart()
    {
        Platform.gameObject.SetActive(true);
        _isAlive = true;
        Platform.PlatformRenderer.material.color = _startColor;
    }

    private void Start()
    {
        _startColor = Platform.PlatformRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAlive && ((1 << other.gameObject.layer) & _targetLayer) != 0)
        {
            ChangePlatformColor(Color.white ,_durationOfDisapperance, DisablePlatform);
        }
    }

    private void ChangePlatformColor(Color color ,float duration, Action Callback)
    {
        Platform.PlatformRenderer.material.DOColor(color, duration).OnComplete(Callback.Invoke);
    }

    private void DisablePlatform()
    {
        Platform.gameObject.SetActive(false);
        _disableEffect.Play();
        _isAlive = false;
    }
}

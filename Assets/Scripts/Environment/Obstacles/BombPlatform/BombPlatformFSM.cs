using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BombPlatformFSM : MonoBehaviour, IRestartable
{
    public float AwakeningDuration { get { return _awakeningDuration; } }
    public float ResetDuration { get { return _resetDuration; } }
    public Color AwakeningColor { get { return _awakeningColor; } }
    public Color DetonateColor { get { return _detonateColor; } }
    public Color StartColor { get { return _startColor; } }

    public readonly SleepState SleepState = new SleepState();
    public readonly AwakeningState AwakeningState = new AwakeningState();
    public readonly ExplosionState ExplosionState = new ExplosionState();
    public readonly ResetState ResetState = new ResetState();

    [SerializeField] private float _explosionDistance;
    [SerializeField] private int _explosionDamage;

    [SerializeField] private float _awakeningDuration;
    [SerializeField] private float _resetDuration;
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField] private Color _awakeningColor;
    [SerializeField] private Color _detonateColor;

    [SerializeField] ParticleSystem _explosionEffect;

    private Platform Platform { get { return _platform = _platform ?? GetComponentInChildren<Platform>(); } }
    private Platform _platform;

    private Renderer Renderer { get { return _renderer = _renderer ?? Platform.PlatformRenderer; } }
    private Renderer _renderer;

    private BombPlatformState _currentState;
    private Color _startColor;

    private async void Start()
    {
        _startColor = Renderer.material.color;
        await TransitionToStateAsync(SleepState, 0.0f);
    }

    public async UniTask TransitionToStateAsync(BombPlatformState state, float duration)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        _currentState = state;
        _currentState.EnterState(this);
    }

    public void ChangeColor(Color color)
    {
        Renderer.material.color = color;
    }

    public void Detonate()
    {
        _explosionEffect.Play();

        var hits = Physics.BoxCastAll(transform.position, Platform.transform.localScale * 0.5f, transform.up, transform.rotation, _explosionDistance, _targetLayer);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent(out ITakingDamage damageable))
                {
                    damageable.TakeDamage(_explosionDamage);
                }
            }
        }
    }

    public async void Restart()
    {
        await TransitionToStateAsync(SleepState, 0.0f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayer) != 0)
        {
            _currentState.OnTriggerStay(this);
        }
    }
}

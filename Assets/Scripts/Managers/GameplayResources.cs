using UnityEngine;
using UnityEngine.Events;

public class GameplayResources : MonoBehaviour, IRestartable
{
    public UnityAction<int> TimeChanged;
    public UnityAction<int, int> PlayerHealthChanged;
    public UnityAction PlayerDied;

    [SerializeField] private int _startHealth;

    private int _currentHealth;

    private int _timer;
    private float _elapsedTime = 0;
    private bool _timerIsAlive;


    public void ChangePlayerHealth(int value)
    {
        _currentHealth += value;
        PlayerHealthChanged?.Invoke(_currentHealth, _startHealth);

        if (_currentHealth <= 0)
        {
            PlayerDied?.Invoke();
        }
    }

    public void TimerStart()
    {
        _timerIsAlive = true;
    }

    public void TimerStop()
    {
        _timerIsAlive = false;
    }

    public void Restart()
    {
        _currentHealth = _startHealth;
        PlayerHealthChanged?.Invoke(_currentHealth, _startHealth);

        _timer = 0;
        TimeChanged?.Invoke(_timer);
    }

    private void Start()
    {
        _currentHealth = _startHealth;
    }

    private void Update()
    {
        if (_timerIsAlive)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= 1.0f)
            {
                _timer++;
                _elapsedTime -= 1.0f;

                TimeChanged?.Invoke(_timer);
            }
        }
    }
}

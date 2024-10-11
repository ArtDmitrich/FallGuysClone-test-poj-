using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class GameplayCanvas : MonoBehaviour
{
    public UnityAction RestartButtonPressed;

    [SerializeField] private TMP_Text _timer;
    [SerializeField] private Slider _healthBar;

    [SerializeField] private GameObject _endGameMenu;
    [SerializeField] private TMP_Text _engameTitle;
    [SerializeField] private TMP_Text _totalTimeValue;
    [SerializeField] private Button _restartButton;

    [SerializeField] private string _winnerText;
    [SerializeField] private string _loserText;

    private GameplayResources _gameplayResources;

    [Inject]
    private void Construct(GameplayResources gameplayResources)
    {
        _gameplayResources = gameplayResources;
    }

    public void OpenEndGameMenu(bool playerWin)
    {
        _timer.gameObject.SetActive(false);
        _healthBar.gameObject.SetActive(false);

        _endGameMenu.SetActive(true);

        _engameTitle.text = playerWin ? _winnerText : _loserText;
        _totalTimeValue.text = _timer.text;
    }

    public void CloseEndGameMenu()
    {
        _endGameMenu.SetActive(false);

        _timer.gameObject.SetActive(true);
        _healthBar.gameObject.SetActive(true);
    }

    private void Start()
    {
        CloseEndGameMenu();
    }

    private void SetCurrentHealth(int currentHealth, int maxHealth)
    {
        var value = (float)currentHealth / maxHealth;
        _healthBar.value = value;
    }

    private void SetElapsedTime(int elapsedTime)
    {
        var minutes = elapsedTime / 60;
        var seconds = elapsedTime % 60;

        _timer.text = $"{minutes:00}:{seconds:00}";
    }

    private void PressRestart()
    {
        RestartButtonPressed?.Invoke();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(PressRestart);

        _gameplayResources.TimeChanged += SetElapsedTime;
        _gameplayResources.PlayerHealthChanged += SetCurrentHealth;
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(PressRestart);

        _gameplayResources.TimeChanged -= SetElapsedTime;
        _gameplayResources.PlayerHealthChanged -= SetCurrentHealth;
    }
}

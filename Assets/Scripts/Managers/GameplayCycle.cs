using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayCycle : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Transform _playerStartPoint;

    [SerializeField] private List<Line> _checkpointLines = new List<Line>();

    [SerializeField] private ObstaclesController _obstaclesController;

    [SerializeField] private float _delayToEndGame;

    private GameplayResources _gameplayResources;
    private GameplayCanvas _gameplayCanvas;

    [Inject]
    private void Construct(GameplayResources gameplayResources, GameplayCanvas gameplayCanvas)
    {
        _gameplayResources = gameplayResources;
        _gameplayCanvas = gameplayCanvas;
    }
    private async void PlayerCrossesLine(LineType lineType)
    {
        switch (lineType)
        {
            case LineType.Start:
                StartGameplay();
                break;
            case LineType.Finish:
                await EndGameplay(true, _delayToEndGame);
                break;
            case LineType.DeadZone:
                await EndGameplay(false, _delayToEndGame);
                break;
        }
    }

    private void StartGameplay()
    {
        _gameplayResources.TimerStart();
    }

    private async UniTask EndGameplay(bool playerWin, float delayTime)
    {
        _gameplayResources.TimerStop();

        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));

        Time.timeScale = 0;

        _gameplayCanvas.OpenEndGameMenu(playerWin);
    }

    private void RestartGame()
    {
        _player.transform.position = _playerStartPoint.position;

        _gameplayResources.Restart();
        _obstaclesController.Restart();

        _gameplayCanvas.CloseEndGameMenu();

        Time.timeScale = 1.0f;
    }

    private async void PlayerDie()
    {
        await EndGameplay(false, _delayToEndGame);
    }

    private void OnEnable()
    {
        _gameplayResources.PlayerDied += PlayerDie;
        _gameplayCanvas.RestartButtonPressed += RestartGame;

        for (int i = 0; i < _checkpointLines.Count; i++)
        {
            _checkpointLines[i].LineCrossed += PlayerCrossesLine;
        }
    }
    private void OnDisable()
    {
        _gameplayResources.PlayerDied -= PlayerDie;
        _gameplayCanvas.RestartButtonPressed -= RestartGame;

        for (int i = 0; i < _checkpointLines.Count; i++)
        {
            _checkpointLines[i].LineCrossed -= PlayerCrossesLine;
        }
    }
}

using UnityEngine;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private GameplayResources _gameplayResources;
    [SerializeField] private GameplayCanvas _gameplayCanvas;

    public override void InstallBindings()
    {
        Container.Bind<InputController>().FromInstance(_inputController).AsSingle();
        Container.Bind<GameplayResources>().FromInstance(_gameplayResources).AsSingle();
        Container.Bind<GameplayCanvas>().FromInstance(_gameplayCanvas).AsSingle();
    }
}
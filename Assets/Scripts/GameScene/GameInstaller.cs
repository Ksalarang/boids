using GameScene.Controllers;
using UnityEngine;
using Zenject;

// ReSharper disable All

namespace GameScene {
public class GameInstaller : MonoInstaller {
    [Header("Controllers")]
    [SerializeField] GameController gameController;
    [SerializeField] SystemManager systemManager;
    [SerializeField] InputController inputController;
    [Header("Views")]
    [SerializeField] SettingsPanel settingsPanel;
    [Header("Misc")]
    [SerializeField] GameSettings gameSettings;
    [SerializeField] new Camera camera;

    public override void InstallBindings() {
        // controllers
        bind(gameController);
        bind(systemManager);
        bind(inputController);
        // views
        bind(settingsPanel);
        // settings
        bind(gameSettings);
        bind(gameSettings.boids);
        bind(gameSettings.flock);
        bind(camera);
    }
    
    void bind<T>(T instance) {
        Container.BindInstance(instance);
    }

    void bind<T>(T instance, object id) {
        Container.Bind<T>().WithId(id).FromInstance(instance);
    }
}
}
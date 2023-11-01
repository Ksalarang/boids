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
    [SerializeField] PredatorController predatorController;
    [SerializeField] BoidFactory boidFactory;
    [SerializeField] CameraController cameraController;
    [Header("Views")]
    [SerializeField] SettingsPanel settingsPanel;
    [Header("Misc")]
    [SerializeField] new Camera camera;

    public override void InstallBindings() {
        // controllers
        bind(gameController);
        bind(systemManager);
        bind(inputController);
        bind(predatorController);
        bind(boidFactory);
        bind(cameraController);
        // views
        bind(settingsPanel);
        // misc
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
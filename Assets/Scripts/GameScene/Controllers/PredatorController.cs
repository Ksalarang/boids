using GameScene.Settings;
using UnityEngine;
using Utils;
using Zenject;

namespace GameScene.Controllers {
public class PredatorController : MonoBehaviour {
    [SerializeField] GameObject predatorPrefab;

    [Inject] DiContainer diContainer;
    [Inject] SystemManager systemManager;

    public Predator createPredator(GameSettings settings) {
        var predator = diContainer.InstantiatePrefabForComponent<Predator>(predatorPrefab);
        predator.transform.rotation = Quaternion.Euler(0, 0, RandomUtils.nextFloat(0, 359));
        predator.boids = systemManager.boids;
        predator.gameSettings = settings;
        predator.settings = settings.predatorSettings;
        return predator;
    }
}
}
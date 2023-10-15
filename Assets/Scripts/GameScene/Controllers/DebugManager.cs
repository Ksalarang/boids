using UnityEngine;
using Zenject;

namespace GameScene.Controllers {
public class DebugManager : MonoBehaviour {
    [Inject] GameSettings settings;
    [Inject] SystemManager systemManager;
    
    Boid[] boids;

    void Start() {
        boids = systemManager.boids;
        var viewAreaDiameter = 2 * settings.flock.localDistance / boids[0].transform.localScale.x;
        foreach (var boid in boids) {
            boid.viewArea.SetActive(settings.debug.viewAreaEnabled);
            boid.viewArea.transform.localScale = new Vector3(viewAreaDiameter, viewAreaDiameter);
        }
    }
}
}
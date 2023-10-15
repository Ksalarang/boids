using UnityEngine;
using Zenject;

namespace GameScene.Controllers {
public class DebugManager : MonoBehaviour {
    [Inject] GameSettings settings;
    [Inject] SystemManager systemManager;
    
    Boid[] boids;

    void Start() {
        boids = systemManager.boids;
        
    }
}
}
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace GameScene.Systems {
public class EvasionSystem : System {
    readonly Boid[] boids;
    readonly Predator predator;
    readonly GameSettings settings;

    public EvasionSystem(Boid[] boids, Predator predator, GameSettings settings) {
        this.boids = boids;
        this.predator = predator;
        this.settings = settings;
    }

    public void update(float deltaTime) {
        var predatorPosition = predator.transform.position;
        foreach (var boid in boids) {
            var distance = boid.transform.position.distanceTo(predatorPosition);
            if (distance < settings.predatorDistance) {
                var fromPredator = boid.transform.position - predatorPosition;
                var angle = MathUtils.vectorToAngle(fromPredator);
                boid.transform.rotation = Quaternion.RotateTowards(
                    boid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    deltaTime * settings.predatorEvasionForce);
            }
        }
    }
}
}
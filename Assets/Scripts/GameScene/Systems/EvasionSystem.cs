using GameScene.Settings;
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace GameScene.Systems {
public class EvasionSystem : System {
    readonly Boid[] boids;
    readonly Predator predator;
    readonly BoidSettings settings;

    public EvasionSystem(Boid[] boids, Predator predator, BoidSettings settings) {
        this.boids = boids;
        this.predator = predator;
        this.settings = settings;
    }

    public void update(float deltaTime) {
        if (!settings.evasionEnabled) return;
        var predatorPosition = predator.transform.position;
        var maxSpeed = settings.maxSpeed;
        var acceleration = settings.maxAcceleration * deltaTime;
        foreach (var boid in boids) {
            var distance = boid.transform.position.distanceTo(predatorPosition);
            boid.isEscapingPredator = distance < settings.predatorViewDistance;
            if (boid.isEscapingPredator) {
                var fromPredator = boid.transform.position - predatorPosition;
                var angle = MathUtils.vectorToAngle(fromPredator);
                var rotation = boid.transform.rotation;
                boid.transform.rotation = Quaternion.RotateTowards(
                    rotation,
                    Quaternion.Euler(0, 0, angle),
                    deltaTime * settings.evasionForce);

                // accelerate away from predator
                var currentAngle = rotation.eulerAngles.z;
                if (currentAngle < 0) currentAngle += 360f;
                if (Mathf.Abs(angle - currentAngle) < 45f) {
                    boid.speed += acceleration;
                    boid.speed = Mathf.Min(boid.speed, maxSpeed);
                }
            }
        }
    }
}
}
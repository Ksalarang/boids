using System;
using GameScene.Settings;
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace GameScene.Systems {
public class EvasionSystem : System {
    readonly Boid[] boids;
    readonly Predator predator;
    readonly BoidSettings settings;
    readonly Vector3 bottomLeft;
    readonly Vector3 topRight;
    readonly float boidHalfSize;

    public EvasionSystem(Boid[] boids, Predator predator, BoidSettings settings, Camera camera) {
        this.boids = boids;
        this.predator = predator;
        this.settings = settings;
        bottomLeft = camera.getBottomLeft();
        topRight = camera.getTopRight();
        boidHalfSize = boids[0].transform.localScale.x / 2;
    }

    public void update(float deltaTime) {
        var predatorPosition = predator.transform.position;
        var wallAvoidanceEnabled = settings.wallAvoidanceEnabled;
        var predatorEvasionEnabled = settings.predatorEvasionEnabled;
        foreach (var boid in boids) {
            if (wallAvoidanceEnabled) avoidWalls(boid, deltaTime);
            if (predatorEvasionEnabled) evadePredator(boid, predatorPosition, deltaTime);
        }
    }

    void avoidWalls(Boid boid, float deltaTime) {
        var currentAngle = boid.transform.eulerAngles.z;
        if (currentAngle < 0) currentAngle += 360;
        var newAngle = currentAngle;
        var frontPosition = boid.transform.position + boidHalfSize * MathUtils.angleToVector(currentAngle);
        const int maxRayCount = 18;
        const float angleStep = 10f;
        const int stepCount = 10;
        const float stepLength = 0.2f;
        var bottomLeft = this.bottomLeft;
        var topRight = this.topRight;
        // cast rays to determine clear path
        for (var i = 0; i < maxRayCount; i++) {
            var sign = i % 2 == 0 ? -1f : 1f;
            newAngle += angleStep * i * sign;
            if (newAngle < 0) newAngle += 360;
            var direction = MathUtils.angleToVector(newAngle);
            var isPathClear = true;
            // follow along the ray to check if the ray collided with a wall
            for (var j = 1; j <= stepCount; j++) {
                var stepPosition = frontPosition + j * stepLength * direction;
                // check if the next step position is outside the walls
                if (stepPosition.x < bottomLeft.x || stepPosition.x > topRight.x ||
                    stepPosition.y < bottomLeft.y || stepPosition.y > topRight.y) {
                    isPathClear = false;
                    break;
                }
            }
            if (isPathClear) break;
        }
        if (Math.Abs(currentAngle - newAngle) < 0.1f) return;
        boid.transform.rotation = Quaternion.RotateTowards(
            boid.transform.rotation,
            Quaternion.Euler(0, 0, newAngle),
            settings.wallAvoidanceForce * deltaTime);
    }

    void evadePredator(Boid boid, Vector3 predatorPosition, float deltaTime) {
        var distance = boid.transform.position.distanceTo(predatorPosition);
        boid.isEscapingPredator = distance < settings.predatorViewDistance;
        if (boid.isEscapingPredator) {
            // steer away from predator
            var fromPredator = boid.transform.position - predatorPosition;
            var angle = MathUtils.vectorToAngle(fromPredator);
            var rotation = boid.transform.rotation;
            boid.transform.rotation = Quaternion.RotateTowards(
                rotation,
                Quaternion.Euler(0, 0, angle),
                settings.predatorEvasionForce * deltaTime);
            
            // accelerate away from predator
            var currentAngle = rotation.eulerAngles.z;
            if (currentAngle < 0) currentAngle += 360f;
            if (Mathf.Abs(angle - currentAngle) < 45f) {
                boid.speed += settings.maxAcceleration * deltaTime;
                boid.speed = Mathf.Min(boid.speed, settings.maxSpeed);
            }
        }
    }
}
}
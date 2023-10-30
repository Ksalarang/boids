using System;
using GameScene.Settings;
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace GameScene.Systems {
public class EvasionSystem : System {
    readonly Boid[] boids;
    readonly Predator predator;
    readonly PredatorSettings predatorSettings;
    readonly BoidSettings boidSettings;
    readonly float boidSize;
    readonly float predatorSize;
    // camera rectangle corners
    Vector3 bottomLeft;
    Vector3 topRight;

    public EvasionSystem(Boid[] boids, Predator predator, GameSettings settings, Camera camera) {
        this.boids = boids;
        this.predator = predator;
        predatorSettings = settings.predatorSettings;
        boidSettings = settings.boidSettings;
        boidSize = boids[0].transform.localScale.x;
        predatorSize = predator.transform.localScale.x;
        setWallCorners(camera, predatorSize / 3);
    }

    void setWallCorners(Camera camera, float offset) {
        bottomLeft = camera.getBottomLeft();
        topRight = camera.getTopRight();
        bottomLeft.x += offset;
        bottomLeft.y += offset;
        topRight.x -= offset;
        topRight.y -= offset;
    }

    public void update(float deltaTime) {
        var predatorPosition = predator.transform.position;
        var wallAvoidanceEnabled = boidSettings.wallAvoidanceEnabled;
        var predatorEvasionEnabled = boidSettings.predatorEvasionEnabled;
        var wallAvoidanceForce = boidSettings.wallAvoidanceForce;
        var boidSize = this.boidSize;
        var boidRayLength = 2 * boidSize;
        foreach (var boid in boids) {
            if (wallAvoidanceEnabled) {
                avoidWalls(boid.transform, boidSize, boidRayLength, deltaTime, wallAvoidanceForce);
            }
            if (predatorEvasionEnabled) {
                evadePredator(boid, predatorPosition, deltaTime);
            }
        }
        avoidWalls(predator.transform, predatorSize, predatorSize, deltaTime, predatorSettings.wallAvoidanceForce);
    }

    void avoidWalls(Transform transform, float bodySize, float rayLength, float deltaTime, float avoidanceForce) {
        var currentAngle = transform.eulerAngles.z;
        if (currentAngle < 0) currentAngle += 360;
        var newAngle = findAngleToAvoidWalls(currentAngle, transform.position, bodySize, rayLength);
        if (Math.Abs(currentAngle - newAngle) < 0.1f) return;
        
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, 0, newAngle),
            avoidanceForce * deltaTime);
    }

    float findAngleToAvoidWalls(float currentAngle, Vector3 position, float bodySize, float rayLength) {
        var newAngle = currentAngle;
        var frontPosition = position + 0.5f * bodySize * MathUtils.angleToVector(currentAngle);
        const int maxRayCount = 19;
        const float angleStep = 10f;
        const float stepLengthFraction = 0.2f;
        var stepLength = rayLength * stepLengthFraction;
        var stepCount = rayLength / stepLength;
        var isPathClear = true;
        // cast rays to determine clear path
        for (var i = 0; i < maxRayCount; i++) {
            var sign = i % 2 == 0 ? -1f : 1f;
            newAngle += angleStep * i * sign;
            if (newAngle < 0) newAngle += 360;
            var direction = MathUtils.angleToVector(newAngle);
            isPathClear = true;
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
            // path in front is clear
            if (isPathClear) break;
        }
        return isPathClear ? newAngle : currentAngle;
    }

    void evadePredator(Boid boid, Vector3 predatorPosition, float deltaTime) {
        var distance = boid.transform.position.distanceTo(predatorPosition);
        boid.isEscapingPredator = distance < boidSettings.predatorViewDistance;
        if (boid.isEscapingPredator) {
            // steer away from predator
            var fromPredator = boid.transform.position - predatorPosition;
            var angle = MathUtils.vectorToAngle(fromPredator);
            var rotation = boid.transform.rotation;
            boid.transform.rotation = Quaternion.RotateTowards(
                rotation,
                Quaternion.Euler(0, 0, angle),
                boidSettings.predatorEvasionForce * deltaTime);
            
            // accelerate away from predator
            var currentAngle = rotation.eulerAngles.z;
            if (currentAngle < 0) currentAngle += 360f;
            if (Mathf.Abs(angle - currentAngle) < 45f) {
                boid.speed += boidSettings.escapeAcceleration * deltaTime;
                boid.speed = Mathf.Min(boid.speed, boidSettings.maxSpeed);
            }
        }
    }
}
}
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using GameScene.Settings;
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace GameScene.Systems {
public class FlockingSystem : System {
    readonly Log log;
    readonly List<Boid> boids;
    readonly GameSettings gameSettings;
    readonly BoidSettings boidSettings;
    readonly List<Boid> neighbors;
    readonly GameObject localCenter;
    readonly GameObject alignmentArrow;
    readonly GameObject separationArrow;

    public FlockingSystem(List<Boid> boids,
        GameSettings gameSettings,
        BoidSettings boidSettings,
        GameObject localCenter,
        GameObject alignmentArrow,
        GameObject separationArrow) {
        log = new Log(GetType());
        this.boids = boids;
        this.gameSettings = gameSettings;
        this.boidSettings = boidSettings;
        neighbors = new List<Boid>(this.boidSettings.count);
        this.localCenter = localCenter;
        this.alignmentArrow = alignmentArrow;
        this.separationArrow = separationArrow;
    }

    public void update(float deltaTime) {
        // reset the debug boid indicators
        var boidCount = boids.Count;
        if (gameSettings.showBoidForces && boidCount > 0) {
            var debugBoid = boids[0];
            var debugBoidPosition = debugBoid.transform.position;
            var debugBoidRotation = debugBoid.transform.rotation;
            localCenter.transform.position = debugBoidPosition;
            alignmentArrow.transform.position = debugBoidPosition;
            separationArrow.transform.position = debugBoidPosition;
            alignmentArrow.transform.rotation = debugBoidRotation;
            separationArrow.transform.rotation = debugBoidRotation;
        }

        // cache values
        //todo: refactor: cache values
        var minSpeed = boidSettings.minSpeed;
        var maxSpeed = boidSettings.maxSpeed;
        var acceleration = boidSettings.baseAcceleration * deltaTime;

        for (var i = 0; i < boidCount; i++) {
            // preparation
            var currentBoid = boids[i];
            var currentBoidPosition = currentBoid.transform.position;
            findNeighbors(currentBoid);
            var neighborCount = neighbors.Count;
            if (neighborCount == 0) continue;

            // calculate average values
            var sameTypeCount = neighborCount;
            var averageDirection = Vector3.zero;
            var averagePosition = Vector3.zero;
            var separationDirection = Vector3.zero;
            var averageSpeed = 0f;
            var shouldSeparate = false;
            foreach (var neighbor in neighbors) {
                var neighborPosition = neighbor.transform.position;
                if (neighbor.distanceTemp < boidSettings.separationDistance) {
                    var fromNeighbor = currentBoidPosition - neighborPosition;
                    if (fromNeighbor == Vector3.zero) continue;
                    separationDirection += fromNeighbor.normalized / fromNeighbor.magnitude;
                    shouldSeparate = true;
                }
                if (boidSettings.colorfulModeEnabled && currentBoid.fishColor != neighbor.fishColor) {
                    sameTypeCount--;
                    continue;
                }
                averageDirection += neighbor.velocity;
                averagePosition += neighborPosition;
                averageSpeed += neighbor.speed;
            }
            // if (sameTypeCount == 0) return;
            var hasSameTypeNeighbors = sameTypeCount > 0;

            if (hasSameTypeNeighbors) {
                averageDirection /= sameTypeCount;
                averagePosition /= sameTypeCount;
                averageSpeed /= sameTypeCount;
            }
            
            // direction alignment
            if (boidSettings.alignmentEnabled && hasSameTypeNeighbors) {
                var angle = MathUtils.vectorToAngle(averageDirection);
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    deltaTime * boidSettings.alignmentForce);

                if (i == 0) alignmentArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            // cohesion
            if (boidSettings.cohesionEnabled && hasSameTypeNeighbors) {
                var cohesionDirection = averagePosition - currentBoidPosition;
                var angle = MathUtils.vectorToAngle(cohesionDirection);
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    deltaTime * boidSettings.cohesionForce);

                if (i == 0) localCenter.transform.position = averagePosition;
            }
            // separation
            if (boidSettings.separationEnabled && shouldSeparate) {
                var angle = MathUtils.vectorToAngle(separationDirection);
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    deltaTime * boidSettings.separationForce);
                
                if (i == 0) separationArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            // speed alignment
            if (boidSettings.speedAlignmentEnabled && hasSameTypeNeighbors) {
                var currentSpeed = currentBoid.speed;
                if (currentSpeed < averageSpeed) {
                    currentSpeed += acceleration;
                } else if (currentSpeed > averageSpeed) {
                    currentSpeed -= acceleration;
                }
                currentBoid.speed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
            }
        }
    }

    void findNeighbors(Boid current) {
        neighbors.Clear();
        var position = current.transform.position;
        foreach (var boid in boids) {
            if (boid == current) continue;
            boid.distanceTemp = boid.transform.position.distanceTo(position);
            if (boid.distanceTemp < boidSettings.viewDistance) neighbors.Add(boid);
        }
    }
}
}
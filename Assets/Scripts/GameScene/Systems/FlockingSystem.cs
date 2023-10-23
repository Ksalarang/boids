using System.Collections.Generic;
using GameScene.Settings;
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace GameScene.Systems {
public class FlockingSystem : System {
    readonly Log log;
    readonly Boid[] boids;
    readonly GameSettings settings;
    readonly List<Boid> neighbors;
    readonly GameObject localCenter;
    readonly GameObject alignmentArrow;
    readonly GameObject separationArrow;

    public FlockingSystem(Boid[] boids,
        GameSettings settings,
        GameObject localCenter,
        GameObject alignmentArrow,
        GameObject separationArrow) {
        log = new Log(GetType());
        this.boids = boids;
        this.settings = settings;
        neighbors = new List<Boid>(settings.count);
        this.localCenter = localCenter;
        this.alignmentArrow = alignmentArrow;
        this.separationArrow = separationArrow;
    }

    public void update(float deltaTime) {
        // reset the debug boid indicators
        if (settings.showLocalCenter) {
            var debugBoid = boids[0];
            var debugBoidPosition = debugBoid.transform.position;
            var debugBoidRotation = debugBoid.transform.rotation;
            localCenter.transform.position = debugBoidPosition;
            alignmentArrow.transform.position = debugBoidPosition;
            separationArrow.transform.position = debugBoidPosition;
            alignmentArrow.transform.rotation = debugBoidRotation;
            separationArrow.transform.rotation = debugBoidRotation;
        }

        for (var i = 0; i < boids.Length; i++) {
            // preparation
            var currentBoid = boids[i];
            var currentBoidPosition = currentBoid.transform.position;
            findNeighbors(currentBoid);
            var neighborCount = neighbors.Count;
            if (neighborCount == 0) continue;

            // calculate average velocity, position and separation direction
            var averageDirection = Vector3.zero;
            var averagePosition = Vector3.zero;
            var separationDirection = Vector3.zero;
            var shouldSeparate = false;
            foreach (var boid in neighbors) {
                averageDirection += boid.velocity;
                var boidPosition = boid.transform.position;
                averagePosition += boidPosition;
                if (boid.distanceTemp < settings.separationDistance) {
                    var fromNeighbor = currentBoidPosition - boidPosition;
                    if (fromNeighbor == Vector3.zero) continue;
                    separationDirection += fromNeighbor.normalized / fromNeighbor.magnitude;
                    shouldSeparate = true;
                }
            }
            averageDirection /= neighborCount;
            averagePosition /= neighborCount;

            // alignment
            if (settings.alignmentEnabled) {
                var angle = MathUtils.vectorToAngle(averageDirection);
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    deltaTime * settings.alignmentForce);

                if (i == 0) alignmentArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            // cohesion
            if (settings.cohesionEnabled) {
                var cohesionDirection = averagePosition - currentBoidPosition;
                var angle = MathUtils.vectorToAngle(cohesionDirection);
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    deltaTime * settings.cohesionForce);

                if (i == 0) localCenter.transform.position = averagePosition;
            }
            // separation
            if (settings.separationEnabled && shouldSeparate) {
                var angle = MathUtils.vectorToAngle(separationDirection);
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    deltaTime * settings.separationForce);

                if (i == 0) separationArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    void findNeighbors(Boid current) {
        neighbors.Clear();
        var position = current.transform.position;
        foreach (var boid in boids) {
            if (boid == current) continue;
            boid.distanceTemp = boid.transform.position.distanceTo(position);
            if (boid.distanceTemp < settings.viewDistance) neighbors.Add(boid);
        }
    }
}
}
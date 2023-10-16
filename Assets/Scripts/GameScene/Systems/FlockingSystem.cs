using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Extensions;

namespace GameScene.Systems {
public class FlockingSystem : System {
    readonly Boid[] boids;
    readonly FlockSettings settings;
    readonly List<Boid> nearbyBoids;
    readonly GameObject localCenter;

    public FlockingSystem(Boid[] boids, FlockSettings settings, GameObject localCenter) {
        this.boids = boids;
        this.settings = settings;
        this.localCenter = localCenter;
        nearbyBoids = new List<Boid>(settings.count);
    }

    public void update(float deltaTime) {
        for (var i = 0; i < boids.Length; i++) {
            var currentBoid = boids[i];
            findNearbyBoids(currentBoid);
            if (i == 0) {
                localCenter.transform.position = currentBoid.transform.position;
                currentBoid.arrow.transform.rotation = currentBoid.transform.rotation;
            }
            if (nearbyBoids.Count == 0) continue;
            // find average vectors
            var averageDirection = Vector3.zero;
            var averagePosition = Vector3.zero;
            var separationForce = Vector3.zero;
            var currentBoidPosition = currentBoid.transform.position;
            foreach (var boid in nearbyBoids) {
                averageDirection += boid.velocity;
                averagePosition += boid.transform.position;
                if (boid.distanceTemp < settings.separationDistance) {
                    var toNeighbor = currentBoidPosition - boid.transform.position;
                    separationForce += toNeighbor.normalized / toNeighbor.magnitude;
                }
            }
            averageDirection /= nearbyBoids.Count;
            averagePosition /= nearbyBoids.Count;
            // add up the vectors
            var resultingVector = Vector3.zero;
            var count = 0;
            if (settings.alignmentEnabled) {
                resultingVector += averageDirection;
                if (i == 0) {
                    var angle = Mathf.Atan2(averageDirection.y, averageDirection.x) * Mathf.Rad2Deg;
                    currentBoid.arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                count++;
            }
            if (settings.cohesionEnabled) {
                resultingVector += averagePosition;
                if (i == 0) localCenter.transform.position = averagePosition;
                count++;
            }
            if (settings.separationEnabled) {
                resultingVector += separationForce;
                count++;
            }
            if (count == 0) continue;
            // find and apply the target angle
            resultingVector /= count;
            var targetAngle = Mathf.Atan2(resultingVector.y, resultingVector.x) * Mathf.Rad2Deg;
            currentBoid.transform.rotation = Quaternion.RotateTowards(
                currentBoid.transform.rotation,
                Quaternion.Euler(0, 0, targetAngle),
                targetAngle * deltaTime * settings.alignmentForce);
        }
    }

    void findNearbyBoids(Boid current) {
        nearbyBoids.Clear();
        var position = current.transform.position;
        foreach (var boid in boids) {
            if (boid == current) continue;
            boid.distanceTemp = boid.transform.position.distanceTo(position);
            if (boid.distanceTemp < settings.viewDistance) {
                nearbyBoids.Add(boid);
            }
        }
    }
}
}
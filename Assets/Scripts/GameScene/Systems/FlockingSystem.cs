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
            // find average values
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
            // apply the rules
            if (settings.alignmentEnabled) { // alignment
                var angle = Mathf.Atan2(averageDirection.y, averageDirection.x) * Mathf.Rad2Deg;
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    angle * deltaTime * settings.alignmentForce);
                if (i == 0) {
                    angle = Mathf.Atan2(averageDirection.y, averageDirection.x) * Mathf.Rad2Deg;
                    currentBoid.arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
            if (settings.cohesionEnabled) { // cohesion
                var angle = Mathf.Atan2(averagePosition.y, averagePosition.x) * Mathf.Rad2Deg;
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    angle * deltaTime * settings.cohesionForce);
                if (i == 0) localCenter.transform.position = averagePosition;
            }
            if (settings.separationEnabled) { // separation
                var angle = Mathf.Atan2(separationForce.y, separationForce.x) * Mathf.Rad2Deg;
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    angle * deltaTime * settings.separationForce);
            }
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
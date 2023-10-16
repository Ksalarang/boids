using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;

namespace GameScene.Systems {
public class FlockingSystem : System {
    readonly Boid[] boids;
    readonly GameSettings settings;
    readonly List<Boid> nearbyBoids;
    readonly GameObject localCenter;
    readonly GameObject alignmentArrow;
    readonly GameObject separationArrow;

    public FlockingSystem(Boid[] boids, GameSettings settings, GameObject localCenter, GameObject alignmentArrow, GameObject separationArrow) {
        this.boids = boids;
        this.settings = settings;
        nearbyBoids = new List<Boid>(settings.count);
        this.localCenter = localCenter;
        this.alignmentArrow = alignmentArrow;
        this.separationArrow = separationArrow;
    }

    public void update(float deltaTime) {
        for (var i = 0; i < boids.Length; i++) {
            var currentBoid = boids[i];
            findNearbyBoids(currentBoid);
            if (i == 0) {
                var currentBoidPosition = currentBoid.transform.position;
                var currentBoidRotation = currentBoid.transform.rotation;
                localCenter.transform.position = currentBoidPosition;
                alignmentArrow.transform.position = currentBoidPosition;
                separationArrow.transform.position = currentBoidPosition;
                alignmentArrow.transform.rotation = currentBoidRotation;
                separationArrow.transform.rotation = currentBoidRotation;
            }
            if (nearbyBoids.Count == 0) continue;
            // find average values
            var averageDirection = Vector3.zero;
            var averagePosition = Vector3.zero;
            var separationForce = Vector3.zero;
            foreach (var boid in nearbyBoids) {
                averageDirection += boid.velocity;
                averagePosition += boid.transform.position;
                if (boid.distanceTemp < settings.separationDistance) {
                    var toNeighbor = currentBoid.transform.position - boid.transform.position;
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
                if (i == 0) alignmentArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
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
                if (i == 0) separationArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
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
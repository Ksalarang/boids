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
        if (settings.showLocalCenter) { // reset debug boid indicators
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
            var currentBoid = boids[i];
            findNearbyBoids(currentBoid);
            if (nearbyBoids.Count == 0) continue;
            
            // calculate average values and separation force
            var averageVelocity = Vector3.zero;
            var averagePosition = Vector3.zero;
            var separationForce = Vector3.zero;
            var currentBoidPosition = currentBoid.transform.position;
            var separationCount = 0;
            foreach (var boid in nearbyBoids) {
                averageVelocity += boid.velocity;
                var boidPosition = boid.transform.position;
                averagePosition += boidPosition;
                if (boid.distanceTemp < settings.separationDistance) {
                    var fromNeighbor = currentBoidPosition - boidPosition;
                    separationForce += fromNeighbor.normalized / fromNeighbor.magnitude;
                    separationCount++;
                }
            }
            averageVelocity /= nearbyBoids.Count;
            averagePosition /= nearbyBoids.Count;
            
            // apply the rules
            if (settings.alignmentEnabled) { // alignment
                var angle = Mathf.Atan2(averageVelocity.y, averageVelocity.x) * Mathf.Rad2Deg;
                if (angle < 0) angle += 360;
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    angle * deltaTime * settings.alignmentForce);

                if (i == 0) {
                    alignmentArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
            if (settings.cohesionEnabled) { // cohesion
                var cohesionForce = averagePosition - currentBoidPosition;
                var angle = Mathf.Atan2(cohesionForce.y, cohesionForce.x) * Mathf.Rad2Deg;
                if (angle < 0) angle += 360;
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    angle * deltaTime * settings.cohesionForce);

                if (i == 0) {
                    localCenter.transform.position = averagePosition;
                }
            }
            if (settings.separationEnabled) { // separation
                var angle = Mathf.Atan2(separationForce.y, separationForce.x) * Mathf.Rad2Deg;
                if (angle < 0) angle += 360;
                currentBoid.transform.rotation = Quaternion.RotateTowards(
                    currentBoid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    angle * deltaTime * settings.separationForce);

                if (i == 0 && separationCount > 0) {
                    separationArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
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
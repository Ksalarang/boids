using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Systems {
public class BorderSystem : System {
    readonly List<Boid> boids;
    readonly Predator predator;
    readonly Vector3 bottomLeft;
    readonly Vector3 topRight;
    readonly float boidOffset;
    readonly float predatorOffset;

    public BorderSystem(List<Boid> boids, Predator predator, Vector3 bottomLeft, Vector3 topRight, float boidSize) {
        this.boids = boids;
        this.predator = predator;
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        boidOffset = boidSize / 2;
        predatorOffset = predator.transform.localScale.x / 2;
    }

    public void update(float deltaTime) {
        foreach (var boid in boids) {
            teleportToOppositeBorderIfOutside(boid.transform, boidOffset);
        }
        teleportToOppositeBorderIfOutside(predator.transform, predatorOffset);
    }

    void teleportToOppositeBorderIfOutside(Transform transform, float offset) {
        var position = transform.position;
        if (position.x + offset < bottomLeft.x) {
            transform.position = new Vector3(topRight.x, position.y, position.z);
        } else if (position.x - offset > topRight.x) {
            transform.position = new Vector3(bottomLeft.x, position.y, position.z);
        }
        if (position.y + offset < bottomLeft.y) {
            transform.position = new Vector3(position.x, topRight.y, position.z);
        } else if (position.y - offset > topRight.y) {
            transform.position = new Vector3(position.x, bottomLeft.y, position.z);
        }
    }
}
}
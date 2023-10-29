using UnityEngine;

namespace GameScene.Systems {
public class BorderSystem : System {
    readonly Boid[] boids;
    readonly Predator predator;
    readonly Vector3 bottomLeft;
    readonly Vector3 topRight;
    readonly float boidOffset;
    readonly float predatorOffset;

    public BorderSystem(Boid[] boids, Predator predator, Vector3 bottomLeft, Vector3 topRight, float boidSize) {
        this.boids = boids;
        this.predator = predator;
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        boidOffset = boidSize / 2;
        predatorOffset = predator.transform.localScale.x / 2;
    }

    public void update(float deltaTime) {
        updateBoids();
        updatePredator();
    }

    void updateBoids() {
        foreach (var boid in boids) {
            var position = boid.transform.position;
            if (position.x + boidOffset < bottomLeft.x) {
                boid.transform.position = new Vector3(topRight.x, position.y, position.z);
            } else if (position.x - boidOffset > topRight.x) {
                boid.transform.position = new Vector3(bottomLeft.x, position.y, position.z);
            }
            if (position.y + boidOffset < bottomLeft.y) {
                boid.transform.position = new Vector3(position.x, topRight.y, position.z);
            } else if (position.y - boidOffset > topRight.y) {
                boid.transform.position = new Vector3(position.x, bottomLeft.y, position.z);
            }
        }
    }

    void updatePredator() {
        var position = predator.transform.position;
        if (position.x + predatorOffset < bottomLeft.x) {
            predator.transform.position = new Vector3(topRight.x, position.y, position.z);
        } else if (position.x - predatorOffset > topRight.x) {
            predator.transform.position = new Vector3(bottomLeft.x, position.y, position.z);
        }
        if (position.y + predatorOffset < bottomLeft.y) {
            predator.transform.position = new Vector3(position.x, topRight.y, position.z);
        } else if (position.y - predatorOffset > topRight.y) {
            predator.transform.position = new Vector3(position.x, bottomLeft.y, position.z);
        }
    }
}
}
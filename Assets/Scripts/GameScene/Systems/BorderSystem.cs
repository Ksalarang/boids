
using UnityEngine;

namespace GameScene.Systems {
public class BorderSystem : System {
    readonly Boid[] boids;
    readonly Vector3 bottomLeft;
    readonly Vector3 topRight;
    readonly float halfSize;

    public BorderSystem(Boid[] boids, Vector3 bottomLeft, Vector3 topRight, float boidSize) {
        this.boids = boids;
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        halfSize = boidSize / 2;
    }

    public void update(float deltaTime) {
        foreach (var boid in boids) {
            var position = boid.transform.position;
            if (position.x + halfSize < bottomLeft.x) {
                boid.transform.position = new Vector3(topRight.x, position.y, position.z);
            } else if (position.x - halfSize > topRight.x) {
                boid.transform.position = new Vector3(bottomLeft.x, position.y, position.z);
            }
            if (position.y + halfSize < bottomLeft.y) {
                boid.transform.position = new Vector3(position.x, topRight.y, position.z);
            } else if (position.y - halfSize > topRight.y) {
                boid.transform.position = new Vector3(position.x, bottomLeft.y, position.z);
            }
        }
    }
}
}
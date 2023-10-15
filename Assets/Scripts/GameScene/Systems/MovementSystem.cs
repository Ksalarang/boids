using UnityEngine;

namespace GameScene.Systems {
public class MovementSystem : System {
    readonly Boid[] boids;
    readonly float speed;

    public MovementSystem(Boid[] boids, float speed) {
        this.boids = boids;
        this.speed = speed;
    }

    public void update(float deltaTime) {
        foreach (var boid in boids) {
            var angleRadians = boid.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            boid.velocity.x = speed * Mathf.Cos(angleRadians);
            boid.velocity.y = speed * Mathf.Sin(angleRadians);
            boid.transform.position += boid.velocity * deltaTime;
        }
    }
}
}
using UnityEngine;

namespace GameScene.Systems {
public class MovementSystem : System {
    readonly Boid[] boids;

    public MovementSystem(Boid[] boids) {
        this.boids = boids;
    }

    public void update(float deltaTime) {
        foreach (var boid in boids) {
            var angleRadians = boid.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            boid.velocity.x = boid.speed * Mathf.Cos(angleRadians);
            boid.velocity.y = boid.speed * Mathf.Sin(angleRadians);
            boid.transform.position += boid.velocity * deltaTime;
        }
    }
}
}
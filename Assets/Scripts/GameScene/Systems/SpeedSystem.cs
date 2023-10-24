using GameScene.Settings;

namespace GameScene.Systems {
public class SpeedSystem : System {
    readonly Boid[] boids;
    readonly BoidSettings settings;

    public SpeedSystem(Boid[] boids, BoidSettings settings) {
        this.boids = boids;
        this.settings = settings;
    }

    public void update(float deltaTime) {
        var targetSpeed = settings.targetSpeed;
        var acceleration = settings.defaultAcceleration * deltaTime;
        foreach (var boid in boids) {
            if (boid.isEscapingPredator) continue;
            var speed = boid.speed;
            if (speed < targetSpeed) {
                boid.speed = speed + acceleration;
            }
        }
    }
}
}
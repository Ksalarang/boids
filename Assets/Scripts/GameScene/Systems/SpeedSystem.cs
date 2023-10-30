using System.Collections.Generic;
using GameScene.Settings;

namespace GameScene.Systems {
public class SpeedSystem : System {
    readonly List<Boid> boids;
    readonly BoidSettings settings;

    public SpeedSystem(List<Boid> boids, BoidSettings settings) {
        this.boids = boids;
        this.settings = settings;
    }

    public void update(float deltaTime) {
        var targetSpeed = settings.targetSpeed;
        var acceleration = settings.baseAcceleration * deltaTime;
        foreach (var boid in boids) {
            var speed = boid.speed;
            if (speed < targetSpeed) {
                boid.speed = speed + acceleration;
            }
        }
    }
}
}
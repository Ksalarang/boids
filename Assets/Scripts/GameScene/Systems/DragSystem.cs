using GameScene.Settings;

namespace GameScene.Systems {
public class DragSystem : System {
    readonly Boid[] boids;
    readonly Predator predator;
    readonly GameSettings settings;

    public DragSystem(Boid[] boids, Predator predator, GameSettings settings) {
        this.boids = boids;
        this.predator = predator;
        this.settings = settings;
    }

    public void update(float deltaTime) {
        var deceleration = settings.dragDeceleration * deltaTime;
        foreach (var boid in boids) {
            boid.speed -= deceleration;
            if (boid.speed < 0) boid.speed = 0;
        }
        // predator
        predator.speed -= deceleration;
        if (predator.speed < 0) predator.speed = 0;
    }
}
}
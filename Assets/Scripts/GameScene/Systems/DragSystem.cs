using GameScene.Settings;
using Utils;

namespace GameScene.Systems {
public class DragSystem : System {
    readonly Predator predator;
    readonly GameSettings settings;

    public DragSystem(Predator predator, GameSettings settings) {
        this.predator = predator;
        this.settings = settings;
    }

    public void update(float deltaTime) {
        updatePredator(deltaTime);
    }

    void updatePredator(float deltaTime) {
        predator.speed -= settings.dragDeceleration * deltaTime;
        if (predator.speed < 0) predator.speed = 0;
    }
}
}
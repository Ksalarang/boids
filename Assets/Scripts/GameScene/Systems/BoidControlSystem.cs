using System.Collections.Generic;
using GameScene.Settings;
using UnityEngine;
using Utils;

namespace GameScene.Systems {
public class BoidControlSystem : System {
    readonly List<Boid> boids;
    readonly GameSettings gameSettings;
    readonly BoidControlSettings settings;
    readonly Camera camera;

    public BoidControlSystem(List<Boid> boids, GameSettings gameSettings, Camera camera) {
        this.boids = boids;
        this.gameSettings = gameSettings;
        settings = gameSettings.boidControlSettings;
        this.camera = camera;
    }

    public void update(float deltaTime) {
        if (gameSettings.settingsPanelEnabled) return;
        
        var mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        var attractingEnabled = Input.GetMouseButton(0);
        var repellingEnabled = Input.GetMouseButton(1);
        var attractingForce = settings.attractingForce * deltaTime;
        var repellingForce = settings.repellingForce * deltaTime;
        
        foreach (var boid in boids) {
            if (attractingEnabled) {
                var direction = mousePosition - boid.transform.position;
                var angle = MathUtils.vectorToAngle(direction);
                boid.transform.rotation = Quaternion.RotateTowards(
                    boid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    attractingForce);
            }
            if (repellingEnabled) {
                var direction = boid.transform.position - mousePosition;
                var angle = MathUtils.vectorToAngle(direction);
                boid.transform.rotation = Quaternion.RotateTowards(
                    boid.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    repellingForce);
            }
        }
    }
}
}
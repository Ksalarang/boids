using System;

namespace GameScene.Settings {
[Serializable]
public class PredatorSettings {
    public float restingSpeed;
    public float maxSpeed;
    public float acceleration;
    public float viewDistance;
    public float chaseSteeringSpeed;
    public float restPeriod;

    public void reset(PredatorSettings defaultSettings) {
        restingSpeed = defaultSettings.restingSpeed;
        maxSpeed = defaultSettings.maxSpeed;
        acceleration = defaultSettings.acceleration;
        viewDistance = defaultSettings.viewDistance;
        chaseSteeringSpeed = defaultSettings.chaseSteeringSpeed;
        restPeriod = defaultSettings.restPeriod;
    }
}
}
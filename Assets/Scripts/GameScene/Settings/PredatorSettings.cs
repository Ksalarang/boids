using System;

namespace GameScene.Settings {
[Serializable]
public class PredatorSettings {
    public float restingSpeed;
    public float maxSpeed;
    public float acceleration;
    public float viewDistance;
    public float chaseSteeringForce;
    public float wallAvoidanceForce;
    public float restPeriod;

    public void reset(PredatorSettings defaultSettings) {
        restingSpeed = defaultSettings.restingSpeed;
        maxSpeed = defaultSettings.maxSpeed;
        acceleration = defaultSettings.acceleration;
        viewDistance = defaultSettings.viewDistance;
        chaseSteeringForce = defaultSettings.chaseSteeringForce;
        wallAvoidanceForce = defaultSettings.wallAvoidanceForce;
        restPeriod = defaultSettings.restPeriod;
    }
}
}
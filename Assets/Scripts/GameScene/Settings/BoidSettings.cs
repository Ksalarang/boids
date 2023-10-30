using System;

namespace GameScene.Settings {
[Serializable]
public class BoidSettings {
    public int count;
    
    public float targetSpeed;
    public float minSpeed;
    public float maxSpeed;
    public float baseAcceleration;
    public float escapeAcceleration;
    
    public float size;
    public float separationDistance;
    public float viewDistance;
    public float predatorViewDistance;
    
    public bool alignmentEnabled;
    public bool cohesionEnabled;
    public bool separationEnabled;
    public bool speedAlignmentEnabled;
    public bool predatorEvasionEnabled;
    public bool wallAvoidanceEnabled;

    public float alignmentForce;
    public float cohesionForce;
    public float separationForce;
    public float predatorEvasionForce;
    public float wallAvoidanceForce;

    public bool typeBasedFlockingEnabled;

    public void reset(BoidSettings defaultSettings) {
        count = defaultSettings.count;
        
        targetSpeed = defaultSettings.targetSpeed;
        minSpeed = defaultSettings.minSpeed;
        maxSpeed = defaultSettings.maxSpeed;
        baseAcceleration = defaultSettings.baseAcceleration;
        escapeAcceleration = defaultSettings.escapeAcceleration;
        
        size = defaultSettings.size;
        separationDistance = defaultSettings.separationDistance;
        viewDistance = defaultSettings.viewDistance;
        predatorViewDistance = defaultSettings.predatorViewDistance;
        
        alignmentEnabled = defaultSettings.alignmentEnabled;
        cohesionEnabled = defaultSettings.cohesionEnabled;
        separationEnabled = defaultSettings.separationEnabled;
        speedAlignmentEnabled = defaultSettings.speedAlignmentEnabled;
        predatorEvasionEnabled = defaultSettings.predatorEvasionEnabled;
        wallAvoidanceEnabled = defaultSettings.wallAvoidanceEnabled;

        alignmentForce = defaultSettings.alignmentForce;
        cohesionForce = defaultSettings.cohesionForce;
        separationForce = defaultSettings.separationForce;
        predatorEvasionForce = defaultSettings.predatorEvasionForce;
        wallAvoidanceForce = defaultSettings.wallAvoidanceForce;

        typeBasedFlockingEnabled = defaultSettings.typeBasedFlockingEnabled;
    }
}
}
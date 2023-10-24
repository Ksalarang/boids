using System;

namespace GameScene.Settings {
[Serializable]
public class BoidSettings {
    public int count;
    public float speed;
    public float size;
    
    public float separationDistance;
    public float viewDistance;
    public float predatorDistance;
    
    public bool alignmentEnabled;
    public bool cohesionEnabled;
    public bool separationEnabled;

    public float alignmentForce;
    public float cohesionForce;
    public float separationForce;
    public float evasionForce;

    public void reset(BoidSettings defaultSettings) {
        count = defaultSettings.count;
        speed = defaultSettings.speed;
        size = defaultSettings.size;
        
        separationDistance = defaultSettings.separationDistance;
        viewDistance = defaultSettings.viewDistance;
        predatorDistance = defaultSettings.predatorDistance;
        
        alignmentEnabled = defaultSettings.alignmentEnabled;
        cohesionEnabled = defaultSettings.cohesionEnabled;
        separationEnabled = defaultSettings.separationEnabled;
        
        alignmentForce = defaultSettings.alignmentForce;
        cohesionForce = defaultSettings.cohesionForce;
        separationForce = defaultSettings.separationForce;
        evasionForce = defaultSettings.evasionForce;
    }
}
}
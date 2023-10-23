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
    public float predatorEvasionForce;
}
}
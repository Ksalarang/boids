using System;

namespace GameScene {
[Serializable]
public class GameSettings {
    public int count = 100;
    public float speed = 2f;
    
    public bool alignmentEnabled = true;
    public bool cohesionEnabled = true;
    public bool separationEnabled = true;
    
    public float alignmentForce = 1f;
    public float cohesionForce = 1f;
    public float separationForce = 1f;
    
    public float size = 0.4f;
    public float viewDistance = 1.2f;
    public float separationDistance = 0.8f;

    public bool showMousePosition;
    public bool showViewArea;
    public bool showLocalCenter;
}
}
using System;

namespace GameScene {
[Serializable]
public class GameSettings {
    public int count;
    public float speed;
    
    public bool alignmentEnabled;
    public bool cohesionEnabled;
    public bool separationEnabled;
    
    public float alignmentForce;
    public float cohesionForce;
    public float separationForce;
    
    public float size;
    public float viewDistance;
    public float separationDistance;

    public bool showMousePosition;
    public bool showViewArea;
    public bool showLocalCenter;

    public GameSettings() {
        reset();
    }
    
    public void reset() {
        count = 100;
        speed = 2f;
        alignmentEnabled = true;
        cohesionEnabled = true;
        separationEnabled = true;
        alignmentForce = 1f;
        cohesionForce = 1f;
        separationForce = 1f;
        size = 0.4f;
        viewDistance = 1.2f;
        separationDistance = 0.8f;
        showMousePosition = false;
        showViewArea = false;
        showLocalCenter = false;
    }
}
}
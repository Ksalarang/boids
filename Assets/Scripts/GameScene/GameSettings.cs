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
    public float separationDistance;
    public float viewDistance;

    public float predatorDistance;
    public float predatorEvasionForce;

    public bool showMousePosition;
    public bool showViewArea;
    public bool showLocalCenter;

    public float gameSpeed;

    public GameSettings() {
        reset();
    }

    public void reset() {
        count = 50;
        speed = 2f;

        alignmentEnabled = true;
        cohesionEnabled = true;
        separationEnabled = true;

        alignmentForce = 30f;
        cohesionForce = 20f;
        separationForce = 50f;

        size = 0.25f;
        separationDistance = 0.25f;
        viewDistance = 0.4f;

        predatorDistance = 2f;
        predatorEvasionForce = 200f;

        showMousePosition = false;
        showViewArea = false;
        showLocalCenter = false;

        gameSpeed = 1f;
    }
}
}
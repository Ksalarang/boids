using System;
using Utils;

namespace GameScene.Settings {
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

    public PredatorSettings predatorSettings;

    [NonSerialized]
    public GameSettings defaultSettings;

    public void reset() {
        if (defaultSettings == null) {
            Log.warn("GameSettings", "cannot reset settings: defaultSettings is null");
            return;
        }
        count = defaultSettings.count;
        speed = defaultSettings.speed;

        alignmentEnabled = defaultSettings.alignmentEnabled;
        cohesionEnabled = defaultSettings.cohesionEnabled;
        separationEnabled = defaultSettings.separationEnabled;

        alignmentForce = defaultSettings.alignmentForce;
        cohesionForce = defaultSettings.cohesionForce;
        separationForce = defaultSettings.separationForce;

        size = defaultSettings.size;
        separationDistance = defaultSettings.separationDistance;
        viewDistance = defaultSettings.viewDistance;

        predatorDistance = defaultSettings.predatorDistance;
        predatorEvasionForce = defaultSettings.predatorEvasionForce;

        showMousePosition = defaultSettings.showMousePosition;
        showViewArea = defaultSettings.showViewArea;
        showLocalCenter = defaultSettings.showLocalCenter;

        gameSpeed = defaultSettings.gameSpeed;

        predatorSettings = defaultSettings.predatorSettings;
    }
}
}
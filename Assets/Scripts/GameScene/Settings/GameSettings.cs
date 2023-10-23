using System;
using Utils;

namespace GameScene.Settings {
[Serializable]
public class GameSettings {
    public bool showMousePosition;
    public bool showViewArea;
    public bool showLocalCenter;

    public float gameSpeed;

    public float dragDeceleration;

    public BoidSettings boidSettings;
    public PredatorSettings predatorSettings;

    [NonSerialized]
    public GameSettings defaultSettings;

    public void reset() {
        if (defaultSettings == null) {
            Log.warn("GameSettings", "cannot reset settings: defaultSettings is null");
            return;
        }

        showMousePosition = defaultSettings.showMousePosition;
        showViewArea = defaultSettings.showViewArea;
        showLocalCenter = defaultSettings.showLocalCenter;

        gameSpeed = defaultSettings.gameSpeed;

        dragDeceleration = defaultSettings.dragDeceleration;

        boidSettings = defaultSettings.boidSettings;
        predatorSettings = defaultSettings.predatorSettings;
    }
}
}
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

    public BoidSettings boidSettings = new();
    public PredatorSettings predatorSettings = new();

    GameSettings defaultSettings;

    public void setDefaultSettings(GameSettings settings) {
        defaultSettings = settings;
    }

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

        boidSettings.reset(defaultSettings.boidSettings);
        predatorSettings.reset(defaultSettings.predatorSettings);
    }
}
}
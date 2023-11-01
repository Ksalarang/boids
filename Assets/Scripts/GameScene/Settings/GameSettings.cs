using System;
using UnityEngine;
using Utils;

namespace GameScene.Settings {
[Serializable]
public class GameSettings {
    public bool showMousePosition;
    public bool showViewAreas;
    public bool showBoidForces;

    //todo: do not save if zero
    public float gameSpeed;

    public float dragDeceleration;

    public BoidSettings boidSettings = new();
    public PredatorSettings predatorSettings = new();
    public BoidControlSettings boidControlSettings = new();

    [NonSerialized] public GameSettings defaultSettings;
    [NonSerialized] public bool settingsPanelEnabled;
    [NonSerialized] public Rect worldRect;

    public void reset() {
        if (defaultSettings == null) {
            Log.warn("GameSettings", "cannot reset settings: defaultSettings is null");
            return;
        }

        showMousePosition = defaultSettings.showMousePosition;
        showViewAreas = defaultSettings.showViewAreas;
        showBoidForces = defaultSettings.showBoidForces;

        gameSpeed = defaultSettings.gameSpeed;

        dragDeceleration = defaultSettings.dragDeceleration;

        boidSettings.reset(defaultSettings.boidSettings);
        predatorSettings.reset(defaultSettings.predatorSettings);
        boidControlSettings.reset(defaultSettings.boidControlSettings);
    }
}
}
using System;

namespace GameScene.Settings {
[Serializable]
public class BoidControlSettings {
    public float attractingForce;
    public float repellingForce;

    public void reset(BoidControlSettings defaultSettings) {
        attractingForce = defaultSettings.attractingForce;
        repellingForce = defaultSettings.repellingForce;
    }
}
}
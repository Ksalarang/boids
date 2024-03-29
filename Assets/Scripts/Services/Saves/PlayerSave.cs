﻿using System;
using GameScene.Settings;
using UnityEngine;

namespace Services.Saves {
[Serializable]
public class PlayerSave {
    public AudioSave audio = new();
    public GameSettings settings = new();

    public string toJson() => JsonUtility.ToJson(this, true);

    public void fromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
}

[Serializable]
public class AudioSave {
    public float soundVolume = 1f;
    public float musicVolume = 1f;
    public bool vibrationEnabled = true;
}
}
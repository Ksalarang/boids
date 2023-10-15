using System;
using UnityEngine;

namespace GameScene {
public class GameSettings : MonoBehaviour {
    public BoidSettings boids;
    public FlockSettings flock;
    public DebugSettings debug;
}

[Serializable]
public class BoidSettings {
    public float size;
    public float speed;
}

[Serializable]
public class FlockSettings {
    public int count;
    public bool alignmentEnabled;
    public bool cohesionEnabled;
    public bool separationEnabled;
    public float viewDistance;
    public float avoidanceDistance;
}

[Serializable]
public class DebugSettings {
    public bool viewAreaEnabled;
}
}
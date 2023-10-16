using System;
using UnityEngine;

namespace GameScene {
public class GameSettings : MonoBehaviour {
    public BoidSettings boids;
    public FlockSettings flock;
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
    public float alignmentForce;
    public float cohesionForce;
    public float separationForce;
    public float viewDistance;
    public float separationDistance;
}
}
using UnityEngine;

namespace GameScene {
public class Boid : MonoBehaviour {
    [SerializeField] public GameObject viewArea;

    [HideInInspector] public new Transform transform;
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public float distanceTemp;
    
    void Awake() {
        transform = base.transform;
    }
}
}
using UnityEngine;

namespace GameScene {
public class Boid : MonoBehaviour {
    [SerializeField] public GameObject viewArea;
    [SerializeField] public GameObject arrow;

    [HideInInspector] public new Transform transform;
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public float distanceTemp;
    
    void Awake() {
        transform = base.transform;
    }
}
}
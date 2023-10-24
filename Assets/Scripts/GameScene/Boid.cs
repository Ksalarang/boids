using UnityEngine;

namespace GameScene {
public class Boid : MonoBehaviour {
    [SerializeField] public GameObject viewArea;

    public new Transform transform;
    public Vector3 velocity;
    public float speed;
    public float distanceTemp;
    public bool isEscapingPredator;
    
    void Awake() {
        transform = base.transform;
    }
}
}
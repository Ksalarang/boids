using Assets.Scripts.GameScene;
using UnityEngine;

namespace GameScene.Controllers {
    public class PredatorController : MonoBehaviour {
        [SerializeField] GameObject predatorPrefab;

        public Predator createPredator() {
            var predator = Instantiate(predatorPrefab).GetComponent<Predator>();
            return predator;
        }
    }
}
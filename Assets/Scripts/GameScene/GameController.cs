using UnityEngine;
using Utils;

namespace GameScene {
public class GameController : MonoBehaviour {

    void Awake() {
        Application.targetFrameRate = 120;
    }
}
}
using TMPro;
using UnityEngine;
using Utils;
using Zenject;

namespace GameScene.Controllers {
public class InputController : MonoBehaviour {
    [SerializeField] TMP_Text mouseLabel;

    [Inject] new Camera camera;

    Log log;

    void Awake() {
        log = new Log(GetType(), true);
    }

    void Update() {
        var screenMousePosition = Input.mousePosition;
        var worldMousePosition = camera.ScreenToWorldPoint(screenMousePosition);
        worldMousePosition.z = 0;
        updateMouseLabel(screenMousePosition, worldMousePosition);
    }

    void updateMouseLabel(Vector3 screenMousePosition, Vector3 worldMousePosition) {
        mouseLabel.transform.position = screenMousePosition;
        mouseLabel.text = $"{(Vector2) screenMousePosition}\n{(Vector2) worldMousePosition}";
    }
}
}
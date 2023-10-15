using TMPro;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace GameScene.Controllers {
public class InputController : MonoBehaviour {
    [SerializeField] TMP_Text mouseLabel;

    [Inject] new Camera camera;
    [Inject] SettingsPanel settingsPanel;

    Log log;
    Vector3 previousScreenMousePosition;

    void Awake() {
        log = new Log(GetType(), true);
    }

    void Update() {
        var screenMousePosition = Input.mousePosition;
        var worldMousePosition = camera.ScreenToWorldPoint(screenMousePosition);
        worldMousePosition.z = 0;
        
        if (!screenMousePosition.approximately(previousScreenMousePosition)) {
            settingsPanel.onMousePositionChanged(screenMousePosition);
            previousScreenMousePosition = screenMousePosition;
        }
        
        updateMouseLabel(screenMousePosition, worldMousePosition);
    }

    void updateMouseLabel(Vector3 screenMousePosition, Vector3 worldMousePosition) {
        mouseLabel.transform.position = screenMousePosition;
        mouseLabel.text = $"{(Vector2) screenMousePosition}\n{(Vector2) worldMousePosition}";
    }

    public void onToggleShowMousePosition(bool value) {
        mouseLabel.gameObject.SetActive(value);
    }
}
}
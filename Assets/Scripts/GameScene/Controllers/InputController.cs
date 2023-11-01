using GameScene.Settings;
using Services.Saves;
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
    [Inject] SystemManager systemManager;
    [Inject] SaveService saveService;

    Log log;
    Vector3 previousScreenMousePosition;
    float previousGameSpeed = 1f;
    // todo: rename
    GameSettings settings;

    public event MouseScrollEvent mouseScrollEvent;

    void Awake() {
        log = new Log(GetType(), true);
        settings = saveService.getSave().settings;
        mouseLabel.gameObject.SetActive(settings.showMousePosition);
    }

    void Update() {
        var screenMousePosition = Input.mousePosition;
        var worldMousePosition = camera.ScreenToWorldPoint(screenMousePosition);
        worldMousePosition.z = 0;

        if (!screenMousePosition.approximately(previousScreenMousePosition)) {
            settingsPanel.onMousePositionChanged(screenMousePosition);
            previousScreenMousePosition = screenMousePosition;
        }
        if (mouseLabel.enabled) updateMouseLabel(screenMousePosition, worldMousePosition);
        updateInput(worldMousePosition);
    }

    void updateMouseLabel(Vector3 screenMousePosition, Vector3 worldMousePosition) {
        mouseLabel.transform.position = screenMousePosition;
        mouseLabel.text = $"{(Vector2) screenMousePosition}\n{(Vector2) worldMousePosition}";
    }

    void updateInput(Vector3 worldMousePosition) {
        if (Input.GetKeyUp(KeyCode.R)) {
            systemManager.onRandomizeBoids();
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            if (settings.gameSpeed > 0) {
                previousGameSpeed = settings.gameSpeed;
                settings.gameSpeed = 0;
            } else {
                settings.gameSpeed = previousGameSpeed;
            }
        }
        var wheelAxis = Input.GetAxis("Mouse ScrollWheel");
        if (!settings.settingsPanelEnabled && wheelAxis != 0) {
            mouseScrollEvent?.Invoke(wheelAxis, worldMousePosition);
        }
    }

    public void onToggleShowMousePosition(bool value) {
        mouseLabel.gameObject.SetActive(value);
    }
}

public delegate void MouseScrollEvent(float scrollAmount, Vector3 mousePosition);
}
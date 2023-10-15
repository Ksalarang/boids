using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace GameScene.Controllers {
public class SettingsPanel : MonoBehaviour {
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image image;
    [Header("Toggles")]
    [SerializeField] Toggle mousePositionToggle;
    [SerializeField] Toggle viewAreaToggle;
    [SerializeField] Toggle localCenterToggle;
    [SerializeField] Toggle alignmentToggle;
    [SerializeField] Toggle cohesionToggle;
    [SerializeField] Toggle separationToggle;

    [Inject] SystemManager systemManager;
    [Inject] InputController inputController;
    [Inject] GameSettings settings;

    Log log;

    float minX;

    void Awake() {
        log = new Log(GetType());
        minX = rectTransform.position.x - rectTransform.rect.width / 2;
        addToggleListeners();
    }

    void addToggleListeners() {
        mousePositionToggle.onValueChanged.AddListener(toggleMousePosition);
        viewAreaToggle.onValueChanged.AddListener(toggleViewArea);
        localCenterToggle.onValueChanged.AddListener(toggleLocalCenter);
        alignmentToggle.onValueChanged.AddListener(toggleAlignment);
        cohesionToggle.onValueChanged.AddListener(toggleCohesion);
        separationToggle.onValueChanged.AddListener(toggleSeparation);
    }

    #region toggle listeners
    void toggleMousePosition(bool value) {
        inputController.onToggleShowMousePosition(value);
    }

    void toggleViewArea(bool value) {
        var boids = systemManager.boids;
        foreach (var boid in boids) {
            boid.viewArea.gameObject.SetActive(value);
        }
    }

    void toggleLocalCenter(bool value) {
        systemManager.onToggleLocalCenter(value);
    }

    void toggleAlignment(bool value) {
        settings.flock.alignmentEnabled = value;
    }

    void toggleCohesion(bool value) {
        settings.flock.cohesionEnabled = value;
    }
    
    void toggleSeparation(bool value) {
        settings.flock.separationEnabled = value;
    }
    #endregion

    public void onMousePositionChanged(Vector3 screenMousePosition) {
        gameObject.SetActive(screenMousePosition.x > minX);
    }
}
}
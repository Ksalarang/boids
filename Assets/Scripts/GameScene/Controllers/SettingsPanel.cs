using Services.Saves;
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
    [Header("Slider")]
    [SerializeField] Slider alignmentForceSlider;
    [SerializeField] Slider cohesionForceSlider;
    [SerializeField] Slider separationForceSlider;
    
    [Inject] SystemManager systemManager;
    [Inject] InputController inputController;
    [Inject] SaveService saveService;

    Log log;
    float minX;
    GameSettings settings;

    void Awake() {
        log = new Log(GetType());
        minX = rectTransform.position.x - rectTransform.rect.width / 2;
        settings = saveService.getSave().settings;
        addListeners();
        initializeFromSave();
    }

    void addListeners() {
        // toggles
        mousePositionToggle.onValueChanged.AddListener(toggleMousePosition);
        viewAreaToggle.onValueChanged.AddListener(toggleViewArea);
        localCenterToggle.onValueChanged.AddListener(toggleLocalCenter);
        alignmentToggle.onValueChanged.AddListener(toggleAlignment);
        cohesionToggle.onValueChanged.AddListener(toggleCohesion);
        separationToggle.onValueChanged.AddListener(toggleSeparation);
        // sliders
        alignmentForceSlider.onValueChanged.AddListener(onAlignmentForceChanged);
        cohesionForceSlider.onValueChanged.AddListener(onCohesionForceChanged);
        separationForceSlider.onValueChanged.AddListener(onSeparationForceChanged);
    }

    void initializeFromSave() {
        // toggles
        mousePositionToggle.isOn = settings.showMousePosition;
        viewAreaToggle.isOn = settings.showViewArea;
        localCenterToggle.isOn = settings.showLocalCenter;
        alignmentToggle.isOn = settings.alignmentEnabled;
        cohesionToggle.isOn = settings.cohesionEnabled;
        separationToggle.isOn = settings.separationEnabled;
        // sliders
        alignmentForceSlider.value = settings.alignmentForce;
        cohesionForceSlider.value = settings.cohesionForce;
        separationForceSlider.value = settings.separationForce;
    }
    
    #region toggle listeners
    void toggleMousePosition(bool value) {
        settings.showMousePosition = value;
        inputController.onToggleShowMousePosition(value);
    }

    void toggleViewArea(bool value) {
        settings.showViewArea = value;
        var boids = systemManager.boids;
        foreach (var boid in boids) {
            boid.viewArea.gameObject.SetActive(value);
        }
    }

    void toggleLocalCenter(bool value) {
        settings.showLocalCenter = value;
        systemManager.onToggleLocalCenter(value);
    }

    void toggleAlignment(bool value) {
        settings.alignmentEnabled = value;
    }

    void toggleCohesion(bool value) {
        settings.cohesionEnabled = value;
    }
    
    void toggleSeparation(bool value) {
        settings.separationEnabled = value;
    }
    #endregion

    #region slider listeners
    void onAlignmentForceChanged(float value) {
        settings.alignmentForce = value;
    }

    void onCohesionForceChanged(float value) {
        settings.cohesionForce = value;
    }

    void onSeparationForceChanged(float value) {
        settings.separationForce = value;
    }
    #endregion

    public void onMousePositionChanged(Vector3 screenMousePosition) {
        gameObject.SetActive(screenMousePosition.x > minX);
    }
}
}
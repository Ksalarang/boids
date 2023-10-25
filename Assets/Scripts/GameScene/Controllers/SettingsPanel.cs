using GameScene.Settings;
using Services.Saves;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace GameScene.Controllers {
public class SettingsPanel : MonoBehaviour {
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GameObject scrollView;
    [Header("Toggles")]
    [SerializeField] Toggle mousePositionToggle;
    [SerializeField] Toggle viewAreaToggle;
    [SerializeField] Toggle localCenterToggle;
    [SerializeField] Toggle alignmentToggle;
    [SerializeField] Toggle cohesionToggle;
    [SerializeField] Toggle separationToggle;
    [SerializeField] Toggle predatorEvasionToggle;
    [SerializeField] Toggle wallAvoidanceToggle;
    [Header("Sliders")]
    [SerializeField] Slider alignmentForceSlider;
    [SerializeField] Slider cohesionForceSlider;
    [SerializeField] Slider separationForceSlider;
    [SerializeField] Slider predatorEvasionForceSlider;
    [SerializeField] Slider wallAvoidanceForceSlider;
    
    [SerializeField] Slider gameSpeedSlider;
    
    [SerializeField] TMP_Text alignmentForceLabel;
    [SerializeField] TMP_Text cohesionForceLabel;
    [SerializeField] TMP_Text separationForceLabel;
    [SerializeField] TMP_Text predatorEvasionForceLabel;
    [SerializeField] TMP_Text wallAvoidanceForceLabel;
    
    [SerializeField] TMP_Text gameSpeedLabel;

    [SerializeField] Button resetButton;
    [SerializeField] Button randomizeBoidsButton;

    [Inject] SystemManager systemManager;
    [Inject] InputController inputController;
    [Inject] SaveService saveService;

    Log log;
    float minX;
    GameSettings gameSettings;
    BoidSettings boidSettings;

    void Awake() {
        log = new Log(GetType());
        minX = rectTransform.position.x - rectTransform.rect.width / 2;
        gameSettings = saveService.getSave().settings;
        boidSettings = gameSettings.boidSettings;
        addListeners();
        updateValues();
    }

    void addListeners() {
        // toggles
        mousePositionToggle.onValueChanged.AddListener(toggleMousePosition);
        viewAreaToggle.onValueChanged.AddListener(toggleViewArea);
        localCenterToggle.onValueChanged.AddListener(toggleLocalCenter);
        alignmentToggle.onValueChanged.AddListener(toggleAlignment);
        cohesionToggle.onValueChanged.AddListener(toggleCohesion);
        separationToggle.onValueChanged.AddListener(toggleSeparation);
        predatorEvasionToggle.onValueChanged.AddListener(toggleEvasion);
        wallAvoidanceToggle.onValueChanged.AddListener(toggleWallAvoidance);
        // sliders
        alignmentForceSlider.onValueChanged.AddListener(onAlignmentForceChanged);
        cohesionForceSlider.onValueChanged.AddListener(onCohesionForceChanged);
        separationForceSlider.onValueChanged.AddListener(onSeparationForceChanged);
        predatorEvasionForceSlider.onValueChanged.AddListener(onEvasionForceChanged);
        wallAvoidanceForceSlider.onValueChanged.AddListener(onWallAvoidanceForceChanged);
        
        gameSpeedSlider.onValueChanged.AddListener(onGameSpeedChanged);
        // buttons
        resetButton.onClick.AddListener(onReset);
        randomizeBoidsButton.onClick.AddListener(onRandomizeBoids);
    }

    void updateValues() {
        // toggles
        mousePositionToggle.isOn = gameSettings.showMousePosition;
        viewAreaToggle.isOn = gameSettings.showViewArea;
        localCenterToggle.isOn = gameSettings.showLocalCenter;
        alignmentToggle.isOn = boidSettings.alignmentEnabled;
        cohesionToggle.isOn = boidSettings.cohesionEnabled;
        separationToggle.isOn = boidSettings.separationEnabled;
        predatorEvasionToggle.isOn = boidSettings.predatorEvasionEnabled;
        wallAvoidanceToggle.isOn = boidSettings.wallAvoidanceEnabled;
        // sliders
        alignmentForceSlider.value = boidSettings.alignmentForce;
        cohesionForceSlider.value = boidSettings.cohesionForce;
        separationForceSlider.value = boidSettings.separationForce;
        predatorEvasionForceSlider.value = boidSettings.predatorEvasionForce;
        wallAvoidanceForceSlider.value = boidSettings.wallAvoidanceForce;
        
        gameSpeedSlider.value = gameSettings.gameSpeed;
        // slider labels
        alignmentForceLabel.text = boidSettings.alignmentForce.ToString("F");
        cohesionForceLabel.text = boidSettings.cohesionForce.ToString("F");
        separationForceLabel.text = boidSettings.separationForce.ToString("F");
        predatorEvasionForceLabel.text = boidSettings.predatorEvasionForce.ToString("F");
        wallAvoidanceForceLabel.text = boidSettings.wallAvoidanceForce.ToString("F");
        
        gameSpeedLabel.text = gameSettings.gameSpeed.ToString("F");
    }

    #region toggle listeners
    void toggleMousePosition(bool value) {
        gameSettings.showMousePosition = value;
        inputController.onToggleShowMousePosition(value);
    }

    void toggleViewArea(bool value) {
        gameSettings.showViewArea = value;
        var boids = systemManager.boids;
        foreach (var boid in boids) boid.viewArea.gameObject.SetActive(value);
    }

    void toggleLocalCenter(bool value) {
        gameSettings.showLocalCenter = value;
        systemManager.onToggleLocalCenter(value);
    }

    void toggleAlignment(bool value) {
        boidSettings.alignmentEnabled = value;
    }

    void toggleCohesion(bool value) {
        boidSettings.cohesionEnabled = value;
    }

    void toggleSeparation(bool value) {
        boidSettings.separationEnabled = value;
    }
    
    void toggleEvasion(bool value) {
        boidSettings.predatorEvasionEnabled = value;
    }
    
    void toggleWallAvoidance(bool value) {
        boidSettings.wallAvoidanceEnabled = value;
    }
    #endregion

    #region slider listeners
    void onAlignmentForceChanged(float value) {
        boidSettings.alignmentForce = value;
        alignmentForceLabel.text = value.ToString("F");
    }

    void onCohesionForceChanged(float value) {
        boidSettings.cohesionForce = value;
        cohesionForceLabel.text = value.ToString("F");
    }

    void onSeparationForceChanged(float value) {
        boidSettings.separationForce = value;
        separationForceLabel.text = value.ToString("F");
    }
    
    void onEvasionForceChanged(float value) {
        boidSettings.predatorEvasionForce = value;
        predatorEvasionForceLabel.text = value.ToString("F");
    }
    
    void onWallAvoidanceForceChanged(float value) {
        boidSettings.wallAvoidanceForce = value;
        wallAvoidanceForceLabel.text = value.ToString("F");
    }

    void onGameSpeedChanged(float value) {
        gameSettings.gameSpeed = value;
        gameSpeedLabel.text = value.ToString("F");
    }
    #endregion

    #region button listeners
    void onReset() {
        gameSettings.reset();
        updateValues();
    }

    void onRandomizeBoids() {
        systemManager.onRandomizeBoids();
    }
    #endregion

    const float frequency = 0.2f;
    float progress;
    bool b = true;

    void Update() {
        if (b) return;
        progress += Time.deltaTime;
        if (progress > frequency) {
            progress = 0f;
            updateValues();
        }
    }

    public void onMousePositionChanged(Vector3 screenMousePosition) {
        scrollView.SetActive(screenMousePosition.x > minX);
    }
}
}
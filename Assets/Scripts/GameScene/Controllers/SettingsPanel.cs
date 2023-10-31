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
    [SerializeField] float sliderMultiplier = 1f;
    [Header("Toggles")]
    [SerializeField] Toggle mousePositionToggle;
    [SerializeField] Toggle viewAreaToggle;
    [SerializeField] Toggle showForcesToggle;
    [SerializeField] Toggle alignmentToggle;
    [SerializeField] Toggle cohesionToggle;
    [SerializeField] Toggle separationToggle;
    [SerializeField] Toggle speedAlignmentToggle;
    [SerializeField] Toggle predatorEvasionToggle;
    [SerializeField] Toggle wallAvoidanceToggle;
    [SerializeField] Toggle typeBasedFlockingToggle;
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
    bool hasStarted;

    void Awake() {
        log = new Log(GetType());
        minX = rectTransform.position.x - rectTransform.rect.width / 2;
        gameSettings = saveService.getSave().settings;
        boidSettings = gameSettings.boidSettings;
        addListeners();
    }

    void Start() {
        hasStarted = true;
    }

    void addListeners() {
        // toggles
        mousePositionToggle.onValueChanged.AddListener(toggleMousePosition);
        viewAreaToggle.onValueChanged.AddListener(toggleViewAreas);
        showForcesToggle.onValueChanged.AddListener(toggleBoidForces);
        alignmentToggle.onValueChanged.AddListener(toggleAlignment);
        cohesionToggle.onValueChanged.AddListener(toggleCohesion);
        separationToggle.onValueChanged.AddListener(toggleSeparation);
        speedAlignmentToggle.onValueChanged.AddListener(toggleSpeedAlignment);
        predatorEvasionToggle.onValueChanged.AddListener(togglePredatorEvasion);
        wallAvoidanceToggle.onValueChanged.AddListener(toggleWallAvoidance);
        typeBasedFlockingToggle.onValueChanged.AddListener(toggleTypeBasedFlocking);
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
        viewAreaToggle.isOn = gameSettings.showViewAreas;
        showForcesToggle.isOn = gameSettings.showBoidForces;
        alignmentToggle.isOn = boidSettings.alignmentEnabled;
        cohesionToggle.isOn = boidSettings.cohesionEnabled;
        separationToggle.isOn = boidSettings.separationEnabled;
        speedAlignmentToggle.isOn = boidSettings.speedAlignmentEnabled;
        predatorEvasionToggle.isOn = boidSettings.predatorEvasionEnabled;
        wallAvoidanceToggle.isOn = boidSettings.wallAvoidanceEnabled;
        typeBasedFlockingToggle.isOn = boidSettings.typeBasedFlockingEnabled;
        // sliders
        alignmentForceSlider.value = boidSettings.alignmentForce / sliderMultiplier;
        cohesionForceSlider.value = boidSettings.cohesionForce / sliderMultiplier;
        separationForceSlider.value = boidSettings.separationForce / sliderMultiplier;
        predatorEvasionForceSlider.value = boidSettings.predatorEvasionForce / sliderMultiplier;
        wallAvoidanceForceSlider.value = boidSettings.wallAvoidanceForce / sliderMultiplier;
        gameSpeedSlider.value = gameSettings.gameSpeed;
        // slider labels
        alignmentForceLabel.text = Mathf.RoundToInt(boidSettings.alignmentForce).ToString();
        cohesionForceLabel.text = Mathf.RoundToInt(boidSettings.cohesionForce).ToString();
        separationForceLabel.text = Mathf.RoundToInt(boidSettings.separationForce).ToString();
        predatorEvasionForceLabel.text = Mathf.RoundToInt(boidSettings.predatorEvasionForce).ToString();
        wallAvoidanceForceLabel.text = Mathf.RoundToInt(boidSettings.wallAvoidanceForce).ToString();

        gameSpeedLabel.text = gameSettings.gameSpeed.ToString("F");
    }

    #region toggle listeners
    void toggleMousePosition(bool value) {
        gameSettings.showMousePosition = value;
        inputController.onToggleShowMousePosition(value);
    }

    void toggleViewAreas(bool value) {
        gameSettings.showViewAreas = value;
        systemManager.onToggleViewAreas(value);
    }

    void toggleBoidForces(bool value) {
        gameSettings.showBoidForces = value;
        systemManager.onToggleBoidForces(value);
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

    void toggleSpeedAlignment(bool value) {
        boidSettings.speedAlignmentEnabled = value;
    }
    
    void togglePredatorEvasion(bool value) {
        boidSettings.predatorEvasionEnabled = value;
        systemManager.onTogglePredatorEvasion(value);
    }
    
    void toggleWallAvoidance(bool value) {
        boidSettings.wallAvoidanceEnabled = value;
    }

    void toggleTypeBasedFlocking(bool value) {
        boidSettings.typeBasedFlockingEnabled = value;
        systemManager.onToggleTypeBasedFlocking(value);
    }
    #endregion

    #region slider listeners
    void onAlignmentForceChanged(float value) {
        value *= sliderMultiplier;
        boidSettings.alignmentForce = value;
        alignmentForceLabel.text = Mathf.RoundToInt(value).ToString();
    }

    void onCohesionForceChanged(float value) {
        value *= sliderMultiplier;
        boidSettings.cohesionForce = value;
        cohesionForceLabel.text = Mathf.RoundToInt(value).ToString();
    }

    void onSeparationForceChanged(float value) {
        value *= sliderMultiplier;
        boidSettings.separationForce = value;
        separationForceLabel.text = Mathf.RoundToInt(value).ToString();
    }
    
    void onEvasionForceChanged(float value) {
        value *= sliderMultiplier;
        boidSettings.predatorEvasionForce = value;
        predatorEvasionForceLabel.text = Mathf.RoundToInt(value).ToString();
    }
    
    void onWallAvoidanceForceChanged(float value) {
        value *= sliderMultiplier;
        boidSettings.wallAvoidanceForce = value;
        wallAvoidanceForceLabel.text = Mathf.RoundToInt(value).ToString();
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

    void OnEnable() {
        if (hasStarted) updateValues();
    }

    public void onMousePositionChanged(Vector3 screenMousePosition) {
        var panelEnabled = screenMousePosition.x > minX;
        gameSettings.settingsPanelEnabled = panelEnabled;
        scrollView.SetActive(panelEnabled);
    }
}
}
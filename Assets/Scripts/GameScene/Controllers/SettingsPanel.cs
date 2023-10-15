using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace GameScene.Controllers {
public class SettingsPanel : MonoBehaviour {
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image image;
    [Header("Toggles")]
    [SerializeField] Toggle showMousePositionToggle;

    [Inject] InputController inputController;

    Log log;
    
    public float minX { get; private set; }

    void Awake() {
        log = new Log(GetType());
        minX = rectTransform.position.x - rectTransform.rect.width / 2;
        addListeners();
    }

    void addListeners() {
        showMousePositionToggle.onValueChanged.AddListener(toggleShowMousePosition);
    }

    void toggleShowMousePosition(bool value) {
        inputController.onToggleShowMousePosition(value);
    }

    public void onMousePositionChanged(Vector3 screenMousePosition) {
        gameObject.SetActive(screenMousePosition.x > minX);
    }
}
}
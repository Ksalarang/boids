using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace GameScene.Controllers {
public class SettingsPanel : MonoBehaviour {
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image image;

    [Inject] new Camera camera;

    Log log;
    float minX;

    void Awake() {
        log = new Log(GetType());
        minX = rectTransform.position.x - rectTransform.rect.width / 2;
    }

    void Update() {
        image.enabled = Input.mousePosition.x > minX;
    }
}
}
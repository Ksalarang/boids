using System.Collections;
using UnityEngine;
using Utils;
using Zenject;

namespace GameScene.Controllers {
public class CameraController : MonoBehaviour {
    [SerializeField] float zoomFactor = 1f;
    [SerializeField] float zoomDuration = 0.2f;
    [SerializeField] float minCameraSize = 1f;
    [SerializeField] float maxCameraSize = 20f;
    
    [Inject] new Camera camera;
    [Inject] InputController inputController;

    Log log;
    float lastEndSize;
    Coroutine zoomCoroutine;


    void Awake() {
        log = new Log(GetType());
    }

    void Start() {
        inputController.mouseScrollEvent += onMouseScroll;
    }

    void onMouseScroll(float scrollAmount, Vector3 worldMousePosition) {
        log.log($"onMouseScroll: {scrollAmount}, position: {worldMousePosition}");
        var sizeDelta = -(scrollAmount * zoomFactor);
        var startSize = camera.orthographicSize;
        var endSize = startSize + sizeDelta;
        if (zoomCoroutine != null) {
            StopCoroutine(zoomCoroutine);
            endSize += lastEndSize - startSize;
        }
        endSize = Mathf.Clamp(endSize, minCameraSize, maxCameraSize);
        zoomCoroutine = StartCoroutine(smoothZoom(startSize, endSize));
        lastEndSize = endSize;
    }

    IEnumerator smoothZoom(float startSize, float endSize) {
        var progress = 0f;
        var duration = Mathf.Max(0.1f, zoomDuration);
        while (progress <= duration) {
            camera.orthographicSize = Mathf.Lerp(startSize, endSize, progress / duration);
            progress += Time.deltaTime;
            yield return null;
        }
        lastEndSize = 0f;
        zoomCoroutine = null;
    }
}
}
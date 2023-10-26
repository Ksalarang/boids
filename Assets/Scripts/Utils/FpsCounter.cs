using TMPro;
using UnityEngine;

namespace Utils {
[RequireComponent(typeof(TMP_Text))]
public class FpsCounter : MonoBehaviour {
    [SerializeField] [Range(0f, 1f)] float expSmoothingFactor = 0.9f;
    [SerializeField] float refreshFrequency = 0.4f;

    float timeSinceUpdate;
    float averageFps = 1f;
    TMP_Text label;

    void Start() {
        label = GetComponent<TMP_Text>();
    }

    void Update() {
        // Exponentially weighted moving average (EWMA)
        averageFps = expSmoothingFactor * averageFps + (1f - expSmoothingFactor) * 1f / Time.unscaledDeltaTime;

        if (timeSinceUpdate < refreshFrequency) {
            timeSinceUpdate += Time.deltaTime;
            return;
        }

        var fps = Mathf.RoundToInt(averageFps);
        label.text = fps.ToString();

        timeSinceUpdate = 0f;
    }
}
}
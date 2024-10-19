using UnityEngine;

public class PulsatingEffect : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float minScale = 1.5f;
    public float maxScale = 3.0f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // calculate a scaling factor based on a sine wave
        float scaleFactor = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2f);

        transform.localScale = originalScale * scaleFactor;
    }
}

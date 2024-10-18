using UnityEngine;

public class PulsatingEffect : MonoBehaviour
{
    public float pulseSpeed = 2f;  // Speed of the pulsating effect
    public float minScale = 1.5f;  // Minimum scale size
    public float maxScale = 2.5f;  // Maximum scale size

    private Vector3 originalScale;

    void Start()
    {
        // Store the original scale of the object
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Calculate a scaling factor based on a sine wave
        float scaleFactor = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2f);

        // Apply the pulsating scale
        transform.localScale = originalScale * scaleFactor;
    }
}

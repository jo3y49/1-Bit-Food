using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasAspect : MonoBehaviour
{
    public float targetAspect = 4f / 3f; // Set this to your desired aspect ratio

    CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        UpdateCanvasScaler();
    }

    void Update()
    {
        // Continuously update the canvas scaler in case the screen size changes (e.g., window resizing)
        UpdateCanvasScaler();
    }

    void UpdateCanvasScaler()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float scaleFactor;

        if (screenAspect < targetAspect) // If the screen is too tall
        {
            scaleFactor = screenAspect / targetAspect;
            // canvasScaler.matchWidthOrHeight = 1; // Match height
        }
        else // If the screen is too wide
        {
            scaleFactor = targetAspect / screenAspect;
            // canvasScaler.matchWidthOrHeight = 0; // Match width
        }

        // Adjust the size of the canvas to maintain the target aspect ratio within the screen
        // canvasScaler.referenceResolution = new Vector2(canvasScaler.referenceResolution.x, canvasScaler.referenceResolution.y * scaleFactor);
    }
}

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraResolutionSetter : MonoBehaviour
{
    private new Camera camera;
    private float targetAspect = 4.0f / 3.0f; // Target aspect ratio
    private Vector2Int lastScreenSize;

    void Start()
    {
        camera = GetComponent<Camera>();
        lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        UpdateCameraRect();
    }

    void Update()
    {
        // Check for a change in resolution
        if (lastScreenSize.x != Screen.width || lastScreenSize.y != Screen.height)
        {
            lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            UpdateCameraRect();
        }
    }

    void UpdateCameraRect()
    {
        // Determine the game window's current aspect ratio
        float windowAspect = (float)Screen.width / Screen.height;
        // Current viewport height should be scaled by this amount
        float scaleHeight = windowAspect / targetAspect;
        Rect rect = camera.rect;

        if (scaleHeight < 1.0f)
        {
            // Letterbox (black bars top and bottom)
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            // Pillarbox (black bars on the sides)
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }
        camera.rect = rect;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class WorldToUI : MonoBehaviour
{
    public Transform targetObject; // The world object to track
    public RectTransform uiElement; // The UI element to move
    public float minY; // Lowest world Y value
    public float maxY; // Highest world Y value
    public float minUIY; // Lowest UI position (in local UI space)
    public float maxUIY; // Highest UI position (in local UI space)

    void Update()
    {
        if (targetObject == null || uiElement == null) return;

        // Normalize world Y between 0 and 1
        float normalizedValue = Mathf.InverseLerp(minY, maxY, targetObject.position.y);

        // Convert to UI Y position
        float newY = Mathf.Lerp(minUIY, maxUIY, normalizedValue);

        // Update UI element without camera influence
        uiElement.anchoredPosition = new Vector2(uiElement.anchoredPosition.x, newY);
    }
}

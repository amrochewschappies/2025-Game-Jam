using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class LavaController : MonoBehaviour
{
    public GameObject LavaPlane;
    public Slider lavaSlider; // Reference to the UI Slider

    private Vector3 TargetPosition;
    private float minHeight;
    private float maxHeight;

    public float MoveSpeed;

    void Start()
    {
        minHeight = LavaPlane.transform.position.y; // Initial lava position
        maxHeight = minHeight + 15; // Target position height
        TargetPosition = new Vector3(LavaPlane.transform.position.x, maxHeight, LavaPlane.transform.position.z);

        if (lavaSlider != null)
        {
            lavaSlider.minValue = 0f;
            lavaSlider.maxValue = 1f;
        }
    }

    void Update()
    {
        // Move the lava upwards
        LavaPlane.transform.position = Vector3.MoveTowards(LavaPlane.transform.position, TargetPosition, MoveSpeed * Time.deltaTime);

        // Update the slider based on the lava's position
        if (lavaSlider != null)
        {
            float normalizedValue = Mathf.InverseLerp(minHeight, maxHeight, LavaPlane.transform.position.y);
            lavaSlider.value = normalizedValue;
        }
        
    }
}

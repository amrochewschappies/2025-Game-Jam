using UnityEngine;

public class DrunkEffect : MonoBehaviour
{
    public float wobbleSpeed = 1.0f;      // Speed of the wobble
    public float wobbleAmount = 1.0f;     // Amount of wobble in position
    public float rotationAmount = 5.0f;   // Max rotation angle in degrees

    private float wobbleTime = 0.0f;
    private Vector3 originalLocalPos;

    void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    void LateUpdate() // Use LateUpdate so it happens after player movement
    {
        wobbleTime += Time.deltaTime * wobbleSpeed;

        // Position wobble
        float wobbleX = Mathf.Sin(wobbleTime) * wobbleAmount;
        float wobbleY = Mathf.Cos(wobbleTime * 0.8f) * wobbleAmount;
        transform.localPosition = originalLocalPos + new Vector3(wobbleX, wobbleY, 0);

        // Rotation wobble
        float rotZ = Mathf.Sin(wobbleTime * 0.5f) * rotationAmount;
        transform.localRotation = Quaternion.Euler(0, 0, rotZ);
    }
}

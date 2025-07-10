using UnityEngine;
using UnityEngine.UI;

public class QuestArrowPointer : MonoBehaviour
{
    [Header("References")]
    public Transform target;                   // The quest target
    public Camera playerCamera;                // Camera for this player
    public RectTransform arrowUI;              // UI arrow image
    public Canvas canvas;                      // Canvas for this player

    [Header("Settings")]
    public float edgeBuffer = 50f;             // Padding from screen edge
    public Vector3 worldOffset = new Vector3(0, 2f, 0);  // Offset above target when visible

    [Header("Smoothing")]
    public float positionSmoothing = 10f;     // Smoothing speed for position
    public float rotationSmoothing = 10f;     // Smoothing speed for rotation

    private RectTransform canvasRect;

    void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        UpdatePointer();
    }

    void UpdatePointer()
    {
        if (target == null || playerCamera == null || arrowUI == null || canvas == null) return;

        Rect camRect = playerCamera.pixelRect;

        Vector3 screenPos = playerCamera.WorldToViewportPoint(target.position + worldOffset);
        bool isVisible = screenPos.z > 0 &&
                         screenPos.x > 0 && screenPos.x < 1 &&
                         screenPos.y > 0 && screenPos.y < 1;

        if (isVisible)
        {
            arrowUI.gameObject.SetActive(true);

            Vector3 worldPosition = target.position + worldOffset;
            Vector3 screenPosition = playerCamera.WorldToScreenPoint(worldPosition);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, playerCamera, out Vector2 localPos))
            {
                Vector3 targetPosition = localPos;
                arrowUI.localPosition = Vector3.Lerp(arrowUI.localPosition, targetPosition, Time.deltaTime * positionSmoothing);

                float currentY = arrowUI.rotation.eulerAngles.y;
                Quaternion targetRotation = Quaternion.Euler(0, currentY, 0);
                arrowUI.rotation = Quaternion.Lerp(arrowUI.rotation, targetRotation, Time.deltaTime * rotationSmoothing);
            }
        }
        else
        {
            arrowUI.gameObject.SetActive(true);

            Vector3 screenCenter = new Vector3(camRect.x + camRect.width / 2f, camRect.y + camRect.height / 2f, 0);
            Vector3 screenPosRaw = playerCamera.WorldToScreenPoint(target.position);

            if (screenPosRaw.z < 0)
                screenPosRaw *= -1; // Flip if behind camera

            Vector3 dir = (screenPosRaw - screenCenter).normalized;

            float canvasWidth = camRect.width;
            float canvasHeight = camRect.height;

            Vector3 screenBounds = new Vector3(canvasWidth / 2 - edgeBuffer, canvasHeight / 2 - edgeBuffer, 0);
            Vector3 pointerPos = dir * Mathf.Min(screenBounds.x / Mathf.Abs(dir.x), screenBounds.y / Mathf.Abs(dir.y));
            pointerPos = screenCenter + pointerPos;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, pointerPos, playerCamera, out Vector2 localPos))
            {
                Vector3 targetPosition = localPos;
                arrowUI.localPosition = Vector3.Lerp(arrowUI.localPosition, targetPosition, Time.deltaTime * positionSmoothing);

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                float currentY = arrowUI.rotation.eulerAngles.y;
                Quaternion targetRotation = Quaternion.Euler(0, currentY, angle - 90);
                arrowUI.rotation = Quaternion.Lerp(arrowUI.rotation, targetRotation, Time.deltaTime * rotationSmoothing);
            }
        }
    }
}

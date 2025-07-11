using UnityEngine;
using System.Collections;
public class DrunkEffect : MonoBehaviour
{
    public float wobbleSpeed = 1.0f;      
    public float wobbleAmount = 1.0f;     
    public float rotationAmount = 5.0f;   
    public float resetDelay = 1.0f;       

    private float wobbleTime = 0.0f;
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;
    private Coroutine resetRoutine;

    void Start()
    {
        originalLocalPos = transform.localPosition;
        originalLocalRot = transform.localRotation;
        this.enabled = false;
    }

    void OnEnable()
    {
        wobbleTime = 0f;
        if (resetRoutine != null)
        {
            StopCoroutine(resetRoutine);
            resetRoutine = null;
        }
    }

    void OnDisable()
    {
        resetRoutine = StartCoroutine(ResetEffectAfterDelay());
    }

    void LateUpdate()
    {
        scheduleEffect();
    }

    private void scheduleEffect()
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

    private IEnumerator ResetEffectAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);

        transform.localPosition = originalLocalPos;
        transform.localRotation = originalLocalRot;

        resetRoutine = null;
    }
}
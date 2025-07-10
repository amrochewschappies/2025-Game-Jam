using UnityEngine;
using UnityEngine.UI;

public class SpeedBoostPickup : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    public float speedBoostAmount = 3f;
    public float boostDuration = 5f;
    public RawImage Player1Overlay;
    public RawImage Player2Overlay;

    void Start()
    {
        Player1Overlay = GameObject.FindGameObjectWithTag("Player1Overlay").GetComponent<RawImage>();   
        Player2Overlay = GameObject.FindGameObjectWithTag("Player2Overlay").GetComponent<RawImage>();   
    }

    void SetOpacity(RawImage rawImage, float alphaValue)
    {
        Color color = rawImage.color;
        color.a = alphaValue / 255f;  // Convert alpha from 0-255 range to 0-1
        rawImage.color = color;
    }


    private void OnTriggerEnter(Collider other)
    {
        MonoBehaviour playerScript = null;

        // Check if the collider has PlayerController
        PlayerController player1 = other.GetComponent<PlayerController>();
        if (player1 != null)
        {
            playerScript = player1;
        }

        // Check if the collider has Player2Controller
        Player2Controller player2 = other.GetComponent<Player2Controller>();
        if (player2 != null)
        {
            playerScript = player2;
        }

        // If we found a valid player script, apply the boost
        if (playerScript != null)
        {
            StartCoroutine(ApplySpeedBoost(playerScript));// Disable only the collider to prevent further pickups
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Optionally, disable the visual part of the pickup (like a sprite, etc.)
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null) renderer.enabled = false;
        }
    }

    private System.Collections.IEnumerator ApplySpeedBoost(MonoBehaviour playerScript)
    {
        float originalSpeed = 0f;

        // Try to call the right Get/Set methods based on type
        if (playerScript is PlayerController player1)
        {
            SetOpacity(Player1Overlay, 187);
            originalSpeed = player1.GetMoveSpeed();
            player1.SetMoveSpeed(originalSpeed + speedBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player1.SetMoveSpeed(originalSpeed);
            SetOpacity(Player1Overlay, 0);
        }
        else if (playerScript is Player2Controller player2)
        {
            SetOpacity(Player2Overlay, 0);
            originalSpeed = player2.GetMoveSpeed();
            player2.SetMoveSpeed(originalSpeed + speedBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player2.SetMoveSpeed(originalSpeed);
            SetOpacity(Player2Overlay, 0);
        }

        Destroy(gameObject); // Optional: remove pickup completely
    }
}

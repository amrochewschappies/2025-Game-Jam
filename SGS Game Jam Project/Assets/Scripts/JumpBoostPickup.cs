using UnityEngine;
using UnityEngine.UI;


public class JumpBoostPickup : MonoBehaviour
{
    [Header("Jump Boost Settings")]
    public float jumpBoostAmount = 5f;
    public float boostDuration = 5f;
    public RawImage Player1Overlay;
    public RawImage Player2Overlay;

    void Start()
    {
        Player1Overlay = GameObject.FindGameObjectWithTag("Player1Overlay").GetComponent<RawImage>();
        Player2Overlay = GameObject.FindGameObjectWithTag("Player2Overlay").GetComponent<RawImage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        MonoBehaviour playerScript = null;

        // Check for PlayerController
        PlayerController player1 = other.GetComponent<PlayerController>();
        if (player1 != null)
        {
            playerScript = player1;
        }

        // Check for Player2Controller
        Player2Controller player2 = other.GetComponent<Player2Controller>();
        if (player2 != null)
        {
            playerScript = player2;
        }

        if (playerScript != null)
        {
            StartCoroutine(ApplyJumpBoost(playerScript));

            // Disable only the collider to prevent further pickups
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

    void SetOpacity(RawImage rawImage, float alphaValue)
    {
        Color color = rawImage.color;
        color.a = alphaValue / 255f;  // Convert alpha from 0-255 range to 0-1
        rawImage.color = color;
    }

    private System.Collections.IEnumerator ApplyJumpBoost(MonoBehaviour playerScript)
    {
        float originalJump = 0f;

        if (playerScript is PlayerController player1)
        {
            SetOpacity(Player1Overlay, 187);
            originalJump = player1.GetJumpForce();
            player1.SetJumpForce(originalJump + jumpBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player1.SetJumpForce(originalJump);
            SetOpacity(Player1Overlay, 0);
        }
        else if (playerScript is Player2Controller player2)
        {
            SetOpacity(Player2Overlay, 187);
            originalJump = player2.GetJumpForce();
            player2.SetJumpForce(originalJump + jumpBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player2.SetJumpForce(originalJump);
            Debug.Log("Player Two Original Jump" + originalJump);
            SetOpacity(Player1Overlay, 0);
        }

        Destroy(gameObject); // Optional: delete the pickup
    }
}

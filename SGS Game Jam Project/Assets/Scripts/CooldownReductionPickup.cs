using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownReductionPickup : MonoBehaviour
{
    [Header("Cooldown Reduction Settings")]
    public float reducedCooldown = 1f;
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
        // Check if it's colliding with Player1 or Player2
        if (other.TryGetComponent(out PlayerController player1))
        {
            // If it's Player1, get TileController
            TileController tileController = player1.GetComponent<TileController>();
            if (tileController != null)
            {
                StartCoroutine(ApplyCooldownReduction(tileController));
                DisablePickup();
            }
        }
        else if (other.TryGetComponent(out Player2Controller player2))
        {
            // If it's Player2, get Player2TileController
            Player2TileController player2TileController = player2.GetComponent<Player2TileController>();
            if (player2TileController != null)
            {
                StartCoroutine(ApplyCooldownReduction(player2TileController));
                DisablePickup();
            }
        }
    }

    void SetOpacity(RawImage rawImage, float alphaValue)
    {
        Color color = rawImage.color;
        color.a = alphaValue / 255f;  // Convert alpha from 0-255 range to 0-1
        rawImage.color = color;
    }


    private void DisablePickup()
    {
        // Disable the collider and renderer for this pickup
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;
    }

    private IEnumerator ApplyCooldownReduction(TileController tileController)
    {
        SetOpacity(Player1Overlay, 187);
        // Store the original cooldown
        float originalCooldown = tileController.GetCooldown();

        // Apply reduced cooldown
        tileController.SetCooldown(reducedCooldown);

        // Wait for duration
        yield return new WaitForSeconds(boostDuration);

        // Restore original cooldown
        tileController.SetCooldown(originalCooldown);
        SetOpacity(Player1Overlay, 0);
        Destroy(gameObject); // Optional: destroy the powerup object
    }

    private IEnumerator ApplyCooldownReduction(Player2TileController player2TileController)
    {
        SetOpacity(Player2Overlay, 187);
        // Store the original cooldown for Player2
        float originalCooldown = player2TileController.GetCooldown();

        // Apply reduced cooldown
        player2TileController.SetCooldown(reducedCooldown);

        // Wait for duration
        yield return new WaitForSeconds(boostDuration);

        // Restore original cooldown
        player2TileController.SetCooldown(originalCooldown);
        SetOpacity(Player1Overlay, 0);
        Destroy(gameObject); // Optional: destroy the powerup object
    }
}

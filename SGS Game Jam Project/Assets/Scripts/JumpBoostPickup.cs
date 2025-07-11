using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


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
    private void Update()
    {
        Vector3 TurnRot = new Vector3(90 * Time.deltaTime, 0, 0); // Y-axis rotation
        transform.Rotate(TurnRot);
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
            StartCoroutine(startEffect());
            AudioManager.Instance.PlaySound("potion-effect", 1 , 0.7f , 0f, 1f);
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

    void ApplyOverlay(RawImage rawImage, Color overlayColor, float alphaValue)
    {
        overlayColor.a = alphaValue / 255f;
        rawImage.color = overlayColor;
    }


    private System.Collections.IEnumerator ApplyJumpBoost(MonoBehaviour playerScript)
    {
        float originalJump = 0f;

        if (playerScript is PlayerController player1)
        {
            ApplyOverlay(Player1Overlay, Color.magenta, 187);
            originalJump = player1.GetJumpForce();
            player1.SetJumpForce(originalJump + jumpBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player1.SetJumpForce(originalJump);
            ApplyOverlay(Player1Overlay, Color.black, 15);
        }
        else if (playerScript is Player2Controller player2)
        {
            ApplyOverlay(Player2Overlay, Color.magenta, 187);
            originalJump = player2.GetJumpForce();
            player2.SetJumpForce(originalJump + jumpBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player2.SetJumpForce(originalJump);
            Debug.Log("Player Two Original Jump" + originalJump);
            ApplyOverlay(Player1Overlay,  Color.black, 15);
        }

        Destroy(gameObject); // Optional: delete the pickup
    }
    IEnumerator startEffect()
    {
        DrunkEffect effect = GameObject.Find("HighEffect").GetComponent<DrunkEffect>();
        effect.enabled = true;
        Debug.Log("jump effect enabled");
        yield return new WaitForSeconds(boostDuration);
        effect.enabled = false;
        Debug.Log("jump effect disabled");
    }
}

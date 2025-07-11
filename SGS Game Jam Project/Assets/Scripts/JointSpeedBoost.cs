using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JointSpeedBoost : MonoBehaviour
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
    private void Update()
    {
        Vector3 TurnRot = new Vector3(0, 90 * Time.deltaTime, 0); // Y-axis rotation
        transform.Rotate(TurnRot);
    }

    void ApplyOverlay(RawImage rawImage, Color overlayColor, float alphaValue)
    {
        overlayColor.a = alphaValue / 255f;
        rawImage.color = overlayColor;
    }


    private void OnTriggerEnter(Collider other)
    {
       // AudioManager.Instance.PlaySound("",1 , 0.3f , 0f , 1f);
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
            StartCoroutine(startEffect());
            AudioManager.Instance.PlaySound("weed-effect", 1 , 1f , 0f, 1f);
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

    private IEnumerator ApplySpeedBoost(MonoBehaviour playerScript)
    {
        float originalSpeed = 0f;

        if (playerScript is PlayerController player1)
        {
            ApplyOverlay(Player1Overlay, Color.green, 187);
            originalSpeed = player1.GetMoveSpeed();
            player1.SetMoveSpeed(originalSpeed - speedBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player1.SetMoveSpeed(originalSpeed);
            ApplyOverlay(Player1Overlay, Color. black, 15);
        }
        else if (playerScript is Player2Controller player2)
        {
            ApplyOverlay(Player2Overlay, Color.green, 187);
            originalSpeed = player2.GetMoveSpeed();
            player2.SetMoveSpeed(originalSpeed - speedBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player2.SetMoveSpeed(originalSpeed);
            ApplyOverlay(Player2Overlay, Color.black, 15);
        }

        Destroy(gameObject);
    }

    IEnumerator startEffect()
    {
        DrunkEffect effect = GameObject.Find("HighEffect").GetComponent<DrunkEffect>();
        effect.enabled = true;
        Debug.Log("joint effect enabled");
        yield return new WaitForSeconds(boostDuration);
        effect.enabled = false;
        Debug.Log("join effect disabled");
    }
}

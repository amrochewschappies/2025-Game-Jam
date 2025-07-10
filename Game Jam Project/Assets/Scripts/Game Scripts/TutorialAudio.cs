using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using System;
using System.Collections;

public class TutorialAudio : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip WelcomeClip;
    public AudioClip JumpClip;
    public AudioClip MoveTileUp;
    public AudioClip MoveTileDown;
    public AudioClip ClosingClip;
    public PlayerController playerController;
    public GameObject Player;
    public PlayerInput playerInput;
    public bool isMouse;
    public bool isController;

    public VideoPlayer videoPlayer;  
    public float[] pauseTimes;

    private bool JumpClipPlayed = false;
    private bool RaiseClipPlayed = false;
    private bool LowerClipPlayed = false;

    void Start()
    {
        playerInput = Player.GetComponent<PlayerInput>();
        DisableController();
        Source.clip = WelcomeClip;

        isMouse = playerInput.currentControlScheme == "Keyboard&Mouse";
        isController = playerInput.currentControlScheme == "Gamepad";

        /*if (videoPlayer != null && pauseTimes.Length > 0)
        {
            videoPlayer.Play();
            StartCoroutine(PauseAtTimesCoroutine());
        }*/
        // Subscribe to events once
        playerInput.actions["Move"].performed += OnMovePerformed;
        playerInput.actions["Jump"].performed += OnJumpPerformed;
        playerInput.actions["TileUp"].performed += OnTileUpPerformed;
        playerInput.actions["TileDown"].performed += OnTileDownPerformed;
        StartCoroutine(PlayAudio());
    }

    IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(1.5f);
        Source.Play();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        if (!JumpClipPlayed && !Source.isPlaying)
        {
            EnableController();
            Source.clip = JumpClip;
            StartCoroutine(PlayAudio());
            JumpClipPlayed = true;
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (!RaiseClipPlayed && !Source.isPlaying && Source.clip == JumpClip)
        {
            Source.clip = MoveTileUp;
            StartCoroutine(PlayAudio());
            RaiseClipPlayed = true;
        }
    }

    private void OnTileUpPerformed(InputAction.CallbackContext ctx)
    {
        if (!LowerClipPlayed && !Source.isPlaying && Source.clip == MoveTileUp)
        {
            Source.clip = MoveTileDown;
            StartCoroutine(PlayAudio());
            LowerClipPlayed = true;
        }
    }

    private void OnTileDownPerformed(InputAction.CallbackContext ctx)
    {
        if (!Source.isPlaying && Source.clip == MoveTileDown)
        {
            Source.clip = ClosingClip;
            StartCoroutine(PlayAudio());
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        playerInput.actions["Move"].performed -= OnMovePerformed;
        playerInput.actions["Jump"].performed -= OnJumpPerformed;
        playerInput.actions["TileUp"].performed -= OnTileUpPerformed;
        playerInput.actions["TileDown"].performed -= OnTileDownPerformed;
    }

    public void DisableController()
    {
        playerController.enabled = false;
    }

    public void EnableController()
    {
        playerController.enabled = true;
    }

    /*IEnumerator PauseAtTimesCoroutine()
    {
        foreach (float pauseTime in pauseTimes)
        {
            yield return new WaitForSeconds(pauseTime);  // Wait for the specified time
            videoPlayer.Pause();  // Pause the video
            Debug.Log("Video paused at " + pauseTime + " seconds.");
        }
    }*/
}

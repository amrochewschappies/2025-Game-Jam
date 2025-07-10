using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public float[] pauseTimes = { 12f, 23f, 34f, 40f };  // The times at which the video will pause
    public PlayerInput playerInput;

    private int currentPauseIndex = 0; // To track the current pause in the sequence

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Cursor.visible = false;
        playerInput.actions["Move"].performed += OnMovePerformed;
        playerInput.actions["Jump"].performed += OnJumpPerformed;
        playerInput.actions["TileUp"].performed += OnTileUpPerformed;
        playerInput.actions["TileDown"].performed += OnTileDownPerformed;

        if (videoPlayer != null && pauseTimes.Length > 0)
        {
            videoPlayer.Play();
            StartCoroutine(PauseAtTimesCoroutine());
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        // Handle move input here if needed
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        // Handle jump input here if needed
    }

    private void OnTileUpPerformed(InputAction.CallbackContext ctx)
    {
        // Handle tile up input here if needed
    }

    private void OnTileDownPerformed(InputAction.CallbackContext ctx)
    {
        // Handle tile down input here if needed
    }

    // This coroutine handles pausing at specific times and waiting for corresponding input
    IEnumerator PauseAtTimesCoroutine()
    {
        // Loop through each pause time
        while (currentPauseIndex < pauseTimes.Length)
        {
            // Wait for the current pause time
            yield return new WaitForSeconds(pauseTimes[currentPauseIndex]);

            // Pause the video at the specified time
            videoPlayer.Pause();
            Debug.Log("Video paused at " + pauseTimes[currentPauseIndex] + " seconds.");

            // Wait for the specific input action based on the current pause index
            yield return new WaitUntil(() => GetActionTriggered(currentPauseIndex));

            // Once the appropriate input is pressed, resume the video
            videoPlayer.Play();
            Debug.Log("Video resumed after input at " + pauseTimes[currentPauseIndex] + " seconds.");

            // Move to the next pause time
            currentPauseIndex++;
        }
        CheckIfVideoComplete();
    }

    // This method returns true if the corresponding input action for the current pause is triggered
    bool GetActionTriggered(int pauseIndex)
    {
        switch (pauseIndex)
        {
            case 0:
                return playerInput.actions["Move"].triggered;
            case 1:
                return playerInput.actions["Jump"].triggered;
            case 2:
                return playerInput.actions["TileUp"].triggered;
            case 3:
                return playerInput.actions["TileDown"].triggered;
            default:
                return false;
        }
    }

    void CheckIfVideoComplete()
    {
        // Check if the video is done playing
        if (videoPlayer.frame >= (long)videoPlayer.frameCount - 1)
        {
            Debug.Log("Video is complete!");
            SceneManager.LoadScene("StartScene");
        }
        else
        {
            Debug.Log("Video is not complete yet.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame ||
           Gamepad.current?.startButton.wasPressedThisFrame == true ||
           Gamepad.current?.selectButton.wasPressedThisFrame == true)
        {
            Debug.Log("Video skipped via input.");
            SceneManager.LoadScene("StartScene");
        }

        if (videoPlayer.isPlaying && videoPlayer.frame >= (long)videoPlayer.frameCount - 1)
        {
            Debug.Log("Video completed in Update!");
            SceneManager.LoadScene("StartScene");
        }
    }
}

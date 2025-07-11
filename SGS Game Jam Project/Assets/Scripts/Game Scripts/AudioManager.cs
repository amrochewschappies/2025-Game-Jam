using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource player1Source;
    [SerializeField] private AudioSource player2Source;

    [SerializeField] private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();
    
    
    [SerializeField] private AudioClip[] jumpClips;
    [SerializeField] private AudioClip[] landClips;
    [SerializeField] private AudioClip[] ThrowClips;
    [SerializeField] private AudioClip[] EffectClips;
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
      
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        
        foreach (AudioClip clip in clips)
        {
            soundLibrary[clip.name] = clip;
            //Debug.Log($"Loaded audio clip: {clip.name}");
        }
        
        if (player1Source == null)
        {
            //Debug.LogError("Player 1 AudioSource is not assigned!");
        }
        if (player2Source == null)
        {
           // Debug.LogError("Player 2 AudioSource is not assigned!");
        }
        
    }

    public void PlaySound(string clipName, int playerNumber, float volume, float delay, float pitch)
    {
        StartCoroutine(PlaySoundWithDelay(clipName, playerNumber, volume, delay, pitch));
    }

    private IEnumerator PlaySoundWithDelay(string clipName, int playerNumber, float volume, float delay, float pitch)
    {
        yield return new WaitForSeconds(delay);
        if (soundLibrary.TryGetValue(clipName, out AudioClip clip))
        {
            AudioSource targetSource = playerNumber == 1 ? player1Source : player2Source;
            if (targetSource != null)
            {
             
                targetSource.clip = clip;
                targetSource.volume = volume;
                targetSource.pitch = pitch; 
                targetSource.Play();
            }
            else
            {
                // Debug.LogError($"AudioSource for Player {playerNumber} is not assigned!");
            }
        }
        else
        {
            // Debug.LogWarning($"AudioClip with name {clipName} not found in sound library.");
        }
    }

    public void StopSound(int playerNumber)
    {
        AudioSource targetSource = playerNumber == 1 ? player1Source : player2Source;

        if (targetSource != null)
        {
            targetSource.Stop();
        }
        else
        {
            // Debug.LogError($"AudioSource for Player {playerNumber} is not assigned!");
        }
    }


    public void RandomiseActionSound(string action, int playerNumber, float volume, float delay, float pitch)
    {
        AudioClip[] selectedClips = null;
        switch (action.ToLower())
        {
            case "jump":
                selectedClips = jumpClips;
                break;
            case "land":
                selectedClips = landClips;
                break;
            default:
                Debug.LogWarning($"Action '{action}' not recognized.");
                return;
        }

        if (selectedClips != null && selectedClips.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, selectedClips.Length);
            PlaySound(selectedClips[randomIndex].name, playerNumber, volume, delay, pitch);
        }
        else
        {
            Debug.LogWarning($"No clips found for {action} action.");
        }
    }
}
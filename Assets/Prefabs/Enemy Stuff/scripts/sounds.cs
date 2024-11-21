using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAutoAudioPlayer : MonoBehaviour
{
    // Array to hold your audio clips
    public AudioClip[] audioClips;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    // Minimum and maximum time between sounds
    public float minTimeBetweenSounds = 0f;
    public float maxTimeBetweenSounds = 1f;

    // Adjustable volume for the audio source
    [Range(0f, 1f)]
    public float volume = 1f;

    void Start()
    {
        // Get the AudioSource component from the prefab
        audioSource = GetComponent<AudioSource>();

        // Ensure the AudioSource exists
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on the GameObject. Please add an AudioSource component.");
            return;
        }

        // Ensure there are audio clips assigned
        if (audioClips.Length == 0)
        {
            Debug.LogError("Please assign audio clips to the 'audioClips' array in the inspector.");
            return;
        }

        // Set initial volume
        audioSource.volume = volume;

        // Start the coroutine to play random sounds
        StartCoroutine(PlayRandomSoundCoroutine());
    }

    private IEnumerator PlayRandomSoundCoroutine()
    {
        while (true)
        {
            // Wait for a random interval
            float waitTime = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
            yield return new WaitForSeconds(waitTime);

            // Play a random sound
            PlayRandomSound();
        }
    }

    public void PlayRandomSound()
    {
        if (audioClips.Length > 0)
        {
            // Pick a random index
            int randomIndex = Random.Range(0, audioClips.Length);

            // Assign the selected clip to the AudioSource
            audioSource.clip = audioClips[randomIndex];

            // Adjust the volume
            audioSource.volume = volume;

            // Play the audio
            audioSource.Play();

            // Debug message
            Debug.Log($"Playing sound: {audioSource.clip.name} at volume: {audioSource.volume}");
        }
    }
}
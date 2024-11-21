using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomAutoAudioPlayer : MonoBehaviour
{
    // Array to hold your audio clips
    public AudioClip[] audioClips;
    public AudioClip footSteps;

    // References to AudioSource components
    private AudioSource gruntAudioSource;
    private AudioSource footstepsAudioSource;

    // Minimum and maximum time between random sounds
    public float minTimeBetweenSounds = 25f;
    public float maxTimeBetweenSounds = 50f;

    public NavMeshAgent agent;

    public GameObject gruntController;
    public GameObject footstepsController;

    // Adjustable volume for the audio sources
    [Range(0f, 1f)]
    public float volume = 1f;

    void Start()
    {
        // Get the AudioSource components
        gruntAudioSource = gruntController.GetComponent<AudioSource>();
        footstepsAudioSource = footstepsController.GetComponent<AudioSource>();

        // Assign the footsteps clip to the footsteps AudioSource
        footstepsAudioSource.clip = footSteps;

        // Set volume for both audio sources
        footstepsAudioSource.volume = volume;
        gruntAudioSource.volume = volume;

        // Ensure the AudioSources exist
        if (gruntAudioSource == null || footstepsAudioSource == null)
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

        // Start the coroutines
        StartCoroutine(PlayRandomSoundCoroutine());
        StartCoroutine(PlayFootstepsCoroutine());
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
            gruntAudioSource.clip = audioClips[randomIndex];

            // Play the audio
            gruntAudioSource.Play();

            // Debug message
            Debug.Log($"Playing grunt sound: {gruntAudioSource.clip.name} at volume: {gruntAudioSource.volume}");
        }
    }

    private IEnumerator PlayFootstepsCoroutine()
    {
        while (true)
        {
            // Check if the agent is moving
            if (agent.velocity.magnitude > 0.1f && !footstepsAudioSource.isPlaying)
            {
                // Play the footsteps sound
                footstepsAudioSource.loop = true; // Ensure the footsteps audio loops
                footstepsAudioSource.Play();
                Debug.Log("Playing footsteps sound.");
            }
            else if (agent.velocity.magnitude <= 0.1f && footstepsAudioSource.isPlaying)
            {
                // Stop the footsteps sound if the agent is idle
                footstepsAudioSource.Stop();
                Debug.Log("Stopping footsteps sound.");
            }

            // Small delay to reduce processing
            yield return new WaitForSeconds(0.1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public float distAwayFromPlayer = 6f;
    public float maxDistFromCenter = 5f;
    public ParticleSystem smokeParticles;
    public AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        setCorrectPosition();
        smokeParticles.Play();
        _audioSource.Play();
    }

    private void setCorrectPosition()
    {
        Transform player = Camera.main.transform;
        Vector3 newPosition;

        // If player is too far from center, spawn shop towards center to avoid getting stuck in hills
        if (Vector3.Distance(player.position, Vector3.zero) >= maxDistFromCenter)
        {
            // Get the direction from the player to the world origin (0, 0, 0)
            Vector3 directionToOrigin = (Vector3.zero - player.position).normalized;
            // Calculate the new position at the specified distance
            newPosition = player.position + directionToOrigin * distAwayFromPlayer;
            print("too far from center");

        }
        else
        {
            newPosition = player.position + player.forward * distAwayFromPlayer;
        }

        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Default");
        // Raycast downwards to find the ground
        if (Physics.Raycast(newPosition + Vector3.up * 10f, Vector3.down, out hit, layerMask))
        {
            // Set the position of the target object
            transform.position = hit.point; 
        }
        else
        {
            newPosition.y = 0f;
            transform.position = newPosition;
        }

        // Make the target object face the player horizontally
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;  // Set the y component to 0 to avoid vertical tilt
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }
}

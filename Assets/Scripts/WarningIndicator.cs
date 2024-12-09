using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningIndicator : MonoBehaviour
{
    public Transform player; // Assign the player transform in the inspector
    public float bobHeight = 0.5f; // Height of the bobbing motion
    public float bobSpeed = 1f; // Speed of the bobbing motion

    private Vector3 _initialPos;

    void Start()
    {
        // Store the initial position of the canvas
        _initialPos = transform.position;

        // Find the player's camera if not set
        if (player == null && Camera.main != null)
        {
            player = Camera.main.transform;
        }
    }

    void Update()
    {
        // Bob up and down
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(_initialPos.x, _initialPos.y + bobOffset, _initialPos.z);

        // Face the player
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Keep the canvas level by ignoring vertical rotation
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}

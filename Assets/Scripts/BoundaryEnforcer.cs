using UnityEngine;

public class MovementBounds : MonoBehaviour
{
    public Collider teleportAreaCollider; // Assign the teleport area collider here
    public Transform playerTransform;     // Reference to the player's transform (e.g., XR Rig)

    private void Update()
    {
        if (!teleportAreaCollider.bounds.Contains(playerTransform.position))
        {
            // If the player moves outside the bounds, snap them back
            Vector3 closestPoint = teleportAreaCollider.ClosestPoint(playerTransform.position);
            playerTransform.position = closestPoint;
        }
    }
}
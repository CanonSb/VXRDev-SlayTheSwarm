using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public GameObject rock;
    private GameObject _activeRock;
    public GameObject attackAreaIndicator;
    private GameObject _activeIndicator;
    public GameObject rockZone;

    public Transform rockStartPoint; // The start point (catapult)
    public Transform rockEndPoint; // The target point (where the rock lands)
    public float arcHeight = 2f; // The height of the arc
    public float duration = 3f; // Time it takes to reach the target
    public float timeBeforeAttack = 3f;

    private float timeElapsed = 0f;
    private Animator _animator;
    private Coroutine atkCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (atkCoroutine != null)
        {
            StopCoroutine(atkCoroutine);
            _animator.Play("CatIdle");
            if (_activeRock != null) Destroy(_activeRock);
            if (_activeIndicator != null) Destroy(_activeIndicator);
        }
        atkCoroutine = StartCoroutine(ThrowRock());
    }

    // TODO: Make 1 catapult always target player, add dmg on rock collision with player
    private IEnumerator ThrowRock()
    {
        // Get a random point within the rock target zone's collider
        BoxCollider zoneCollider = rockZone.GetComponent<BoxCollider>();
        // Spawn attack warning indicator at random point
        if (zoneCollider != null)
        {
            Vector3 randomPosition = GetRandomPointInBounds(zoneCollider.bounds);
            randomPosition.y += 0.1f; // Adjust height to avoid spawning inside the ground

            // Instantiate the Indicator
            _activeIndicator = Instantiate(attackAreaIndicator, randomPosition, attackAreaIndicator.transform.rotation);
            if (_activeIndicator != null) rockEndPoint = _activeIndicator.transform;
        }
        // Wait before attack
        yield return new WaitForSeconds(timeBeforeAttack);

        // Start animation
        _animator.Play("CatAttack");

        // Wait for specific point in animation
        yield return new WaitForSeconds(0.13f);

        // Instantiate Rock
        _activeRock = Instantiate(rock, rockStartPoint);
        _activeRock.transform.SetParent(null);

        // Lerp rock to target position with an arc
        timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            // Apply a smooth easing function for faster start and end
            // t = Mathf.Sin(t * Mathf.PI * 0.5f); // Acceleration at the start
            // Lerp horizontally from start to end
            Vector3 horizontalPos = Vector3.Lerp(rockStartPoint.position, rockEndPoint.position, t);
            // Calculate vertical position based on an arc (parabola)
            float verticalPos = Mathf.Lerp(rockStartPoint.position.y, rockEndPoint.position.y, t) + arcHeight * Mathf.Sin(t * Mathf.PI);
            // Set the object's position
            _activeRock.transform.position = new Vector3(horizontalPos.x, verticalPos, horizontalPos.z);

            yield return null;
        }

        // Destroy rock and indicator once finished
        Destroy(_activeRock);
        Destroy(_activeIndicator);
    }


    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        // Start the point slightly above the max Y bound to ensure the raycast hits the ground
        Vector3 randomPoint = new Vector3(x, bounds.max.y + 1, z);
        int layerMask = LayerMask.NameToLayer("RockTargetZone");

        // Raycast downwards to find the ground
        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, bounds.size.y * 2, layerMask))
        {
            return hit.point; // Return the exact point on the ground
        }

        // If raycast fails, return a default safe position (e.g., center of bounds)
        return new Vector3(x, bounds.min.y, z);
    }
}

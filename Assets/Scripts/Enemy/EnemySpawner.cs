using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Attached game objects will have enemies spawned on the ground inside their collider.")]
    public GameObject[] spawnZones; // Array to store spawning zone GameObjects
    public GameObject enemyPrefab;  // Enemy prefab to spawn

    public float spawnInterval = 5f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
           yield return new WaitForSeconds(spawnInterval);  
           SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        // Choose a random spawn zone
        GameObject selectedZone = spawnZones[Random.Range(0, spawnZones.Length)];

        // Get a random point within the selected zone's collider
        Collider zoneCollider = selectedZone.GetComponent<Collider>();
        if (zoneCollider != null)
        {
            Vector3 randomPosition = GetRandomPointInBounds(zoneCollider.bounds);
            randomPosition.y += 1; // Adjust height to avoid spawning inside the ground

            // Instantiate the enemy
            GameObject spawnedEnemy = Instantiate(enemyPrefab, randomPosition, enemyPrefab.transform.rotation);
        }
    }


    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        // Start the point slightly above the max Y bound to ensure the raycast hits the ground
        Vector3 randomPoint = new Vector3(x, bounds.max.y + 1, z);
        int layerMask = LayerMask.NameToLayer("SpawnZone");

        // Raycast downwards to find the ground
        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, bounds.size.y * 2, layerMask))
        {
            return hit.point; // Return the exact point on the ground
        }

        // If raycast fails, return a default safe position (e.g., center of bounds)
        return new Vector3(x, bounds.min.y, z);
    }
}

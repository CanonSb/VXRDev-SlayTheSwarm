using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 5f;
    public List<GameObject> enemyList;
    public List<int> spawnWeights;

    [Tooltip("Attached game objects will have enemies spawned on the ground inside their collider.")]
    public GameObject[] spawnZones; // Array to store spawning zone GameObjects

    public List<GameObject> spawnedEnemies;


    // Wait for duration, then spawn random enemy based on their spawn weights
    public IEnumerator SpawnEnemies()
    {
        while (true)
        {
           yield return new WaitForSeconds(spawnInterval); 

           GameObject randomEnemy = GetWeightedRandomEnemy(enemyList, spawnWeights);
           SpawnEnemy(randomEnemy);
        }
    }

    public void SpawnEnemy(GameObject enemy)
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
            GameObject spawnedEnemy = Instantiate(enemy, randomPosition, enemy.transform.rotation);
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


    GameObject GetWeightedRandomEnemy(List<GameObject> enemyList, List<int> spawnWeights)
    {
        // Calculate the total weight
        int totalWeight = 0;
        for (int i = 0; i < spawnWeights.Count; i++)
        {
            totalWeight += spawnWeights[i];
        }

        // Generate a random number in the range [0, totalWeight)
        int randomValue = Random.Range(0, totalWeight);

        // Determine which enemy to select based on the random value
        int cumulativeWeight = 0;
        for (int i = 0; i < enemyList.Count; i++)
        {
            cumulativeWeight += spawnWeights[i];
            if (randomValue < cumulativeWeight)
            {
                return enemyList[i];
            }
        }

        // Fallback (should never hit this point if weights are valid)
        return null;
    }
}

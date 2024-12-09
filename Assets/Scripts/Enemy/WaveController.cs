using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class WaveController : MonoBehaviour
{
    public TMP_Text timerText;
    public Slider waveBar;
    public int waveNum;
    public EnemySpawner spawner;

    [Header("Enemy Prefabs")]
    public GameObject goblinGreen;
    public GameObject goblinYellow;
    public GameObject goblinRed;

    private float _elapsedTime = 0;
    private float _waveTime = 0;
    private Coroutine enemySpawning;

    void Update()
    {
        // Increment the elapsed time
        _elapsedTime += Time.deltaTime;
        _waveTime += Time.deltaTime;

        // Update functions
        UpdateTimerText();

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            StartNextWave();
        }

        if (Keyboard.current.deleteKey.wasPressedThisFrame)
        {
            GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag("Goblin");
            foreach (GameObject enemy in spawnedEnemies)
            {
                EnemyVitals vitals = enemy.GetComponent<EnemyVitals>();
                if (vitals != null) vitals.OnGameObjectDestroyed(enemy);
            }
        }
    }

    private void UpdateTimerText()
    {
        // Calculate minutes and seconds
        int displayMinutes = Mathf.FloorToInt(_elapsedTime / 60);
        int displaySeconds = Mathf.FloorToInt(_elapsedTime % 60);

        float wavePercentage = Mathf.Min(1f, _waveTime / 60f);

        // Format the text
        timerText.text = string.Format("wave {0}               {1:00}:{2:00}", waveNum, displayMinutes, displaySeconds);
        waveBar.value = wavePercentage;
    }

    // Start next wave and end after duration
    public void StartNextWave()
    {
        // Stop enemy spawning if skipping through waves
        if (enemySpawning != null) EndWave();
        
        // increment wave num and reset wave timer
        float duration = 60f;
        waveNum++;
        _waveTime = 0;

        print(string.Format("Starting wave {0}.", waveNum));
        // Start coroutine to update wave settings
        StartCoroutine(UpdateWaveOverTime(duration));
        // Start enemy spawning coroutine
        enemySpawning = StartCoroutine(spawner.SpawnEnemies());
    }

    // End wave
    public void EndWave()
    {
        print(string.Format("Wave {0} has ended.", waveNum));
        StopCoroutine(enemySpawning);
    }


    // TODO: Waves are hard-coded for now. May change later to make it easier to edit them
    // Each wave is currently split into 3 segments
    // For simplicity the sum of all spawn weights will be 100
    private IEnumerator UpdateWaveOverTime(float duration)
    {
        switch (waveNum)
        {
            // Start easy with green
            case 1:
                spawner.enemyList = new List<GameObject> {goblinGreen};
                spawner.spawnWeights = new List<int> {100};
                spawner.spawnInterval = 3.25f;
                yield return new WaitForSeconds(duration / 2);

                spawner.spawnInterval = 3f;
                yield return new WaitForSeconds(duration / 2);
                break;
            // Introduce yellow
            case 2:
                spawner.enemyList = new List<GameObject> {goblinGreen, goblinYellow};
                spawner.spawnWeights = new List<int> {70, 30};
                spawner.spawnInterval = 3;
                yield return new WaitForSeconds(duration / 2);

                spawner.spawnWeights = new List<int> {50, 50};
                yield return new WaitForSeconds(duration / 2);
                break;
            // Yellow swarm
            case 3:
                spawner.enemyList = new List<GameObject> {goblinGreen, goblinYellow};
                spawner.spawnWeights = new List<int> {40, 60};
                spawner.spawnInterval = 2.75f;
                yield return new WaitForSeconds(duration / 2);

                spawner.spawnWeights = new List<int> {0, 100};
                spawner.spawnInterval = 2.5f;
                yield return new WaitForSeconds(duration / 2);
                break;
            // Introduce red
            case 4:
                spawner.enemyList = new List<GameObject> {goblinGreen, goblinYellow, goblinRed};
                spawner.spawnWeights = new List<int> {50, 30, 20};
                spawner.spawnInterval = 2.5f;
                yield return new WaitForSeconds(duration / 2);

                spawner.spawnWeights = new List<int> {25, 35, 40};
                spawner.spawnInterval = 2.25f;
                yield return new WaitForSeconds(duration / 2);
                break;
            // The rumbling
            case 5:
                spawner.enemyList = new List<GameObject> {goblinGreen, goblinYellow, goblinRed};
                spawner.spawnWeights = new List<int> {0, 0, 100};
                spawner.spawnInterval = 2.25f;
                yield return new WaitForSeconds(duration / 2);

                spawner.spawnWeights = new List<int> {0, 30, 70};
                spawner.spawnInterval = 2f;
                yield return new WaitForSeconds(duration / 2);
                break;
            default:
                break;
        }
        EndWave();
    }
}

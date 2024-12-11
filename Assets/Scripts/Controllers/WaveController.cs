using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Net;

public class WaveController : MonoBehaviour
{
    public TMP_Text timerText;
    public Slider waveBar;
    public int waveNum;
    public EnemySpawner spawner;
    public float waveDuration = 60f;

    [Header("Enemy Prefabs")]
    public GameObject goblinGreen;
    public GameObject goblinYellow;
    public GameObject goblinRed;
    [Header("Catapult Array")]
    public GameObject[] catapults;
    [Header("Shop")]
    public GameObject shop;
    [Header("Day Night Cycle")]
    public DayNightCycleManager DNCycleManager;

    private float _elapsedTime = 0;
    private float _waveTime = 0;
    private Coroutine enemySpawning;
    private Coroutine waveUpdating;
    private Coroutine dayCoroutine;

    void Start()
    {
        DNCycleManager.SetDay();
    }

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
            KillRemainingGoblins();
            StartCoroutine(TriggerCatapultAttacks());
        }
    }

    private void UpdateTimerText()
    {
        // Don't start counting until game is started
        if (waveNum < 1) return;
        // Calculate minutes and seconds
        int displayMinutes = Mathf.FloorToInt(_elapsedTime / 60);
        int displaySeconds = Mathf.FloorToInt(_elapsedTime % 60);

        float wavePercentage = Mathf.Min(1f, _waveTime / waveDuration);

        // Format the text
        timerText.text = string.Format("wave {0}               {1:00}:{2:00}", waveNum, displayMinutes, displaySeconds);
        waveBar.value = wavePercentage;
    }

    // Start next wave and end after duration
    public void StartNextWave()
    {
        // Stop enemy spawning if skipping through waves
        // if (enemySpawning != null) EndWave();
        if (shop != null) shop.SetActive(false);
        
        // increment wave num and reset wave timer
        waveNum++;
        print(string.Format("Starting wave {0}.", waveNum));

        dayCoroutine = StartCoroutine(ChangeToDay());
    }

    // End wave
    public void EndWave()
    {
        print(string.Format("Wave {0} has ended.", waveNum));
        StartCoroutine(ChangeToNight());
        StopCoroutine(enemySpawning);
    }


    // TODO: Waves are hard-coded for now. May change later to make it easier to edit them
    // Each wave is currently split into 3 segments
    // For simplicity the sum of all spawn weights will be 100

    // TODO: Tweak wave difficulty and add more catapult attacks
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
                SpawnCatapults();
                spawner.enemyList = new List<GameObject> {goblinGreen, goblinYellow};
                spawner.spawnWeights = new List<int> {40, 60};
                spawner.spawnInterval = 2.75f;
                yield return new WaitForSeconds(duration / 2);
                StartCoroutine(TriggerCatapultAttacks());

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
                StartCoroutine(TriggerCatapultAttacks());

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
                StartCoroutine(TriggerCatapultAttacks());

                spawner.spawnWeights = new List<int> {0, 30, 70};
                spawner.spawnInterval = 2f;
                yield return new WaitForSeconds(duration / 2);
                break;
            case 6:
                spawner.enemyList = new List<GameObject> {goblinGreen, goblinYellow, goblinRed};
                spawner.spawnWeights = new List<int> {33, 33, 33};
                spawner.spawnInterval = 1.75f;
                yield return new WaitForSeconds(duration / 2);
                StartCoroutine(TriggerCatapultAttacks());

                spawner.spawnWeights = new List<int> {33, 33, 33};
                spawner.spawnInterval = 1.5f;
                yield return new WaitForSeconds(duration / 2);
                break;
            case 7:
                spawner.enemyList = new List<GameObject> {goblinGreen, goblinYellow, goblinRed};
                spawner.spawnWeights = new List<int> {0, 0, 100};
                spawner.spawnInterval = 1.5f;
                yield return new WaitForSeconds(duration / 4);
                StartCoroutine(TriggerCatapultAttacks());
                yield return new WaitForSeconds(duration / 4);
                spawner.spawnWeights = new List<int> {0, 0, 100};
                spawner.spawnInterval = 1.25f;
                StartCoroutine(TriggerCatapultAttacks());
                yield return new WaitForSeconds(duration / 4);
                StartCoroutine(TriggerCatapultAttacks());
                yield return new WaitForSeconds(duration / 4);
                break;
            case 8:
                spawner.enemyList = new List<GameObject> {goblinGreen, goblinYellow, goblinRed};
                spawner.spawnWeights = new List<int> {0, 100, 0};
                spawner.spawnInterval = 1f;
                yield return new WaitForSeconds(duration / 4);
                StartCoroutine(TriggerCatapultAttacks());
                yield return new WaitForSeconds(duration / 4);
                spawner.spawnWeights = new List<int> {0, 100, 0};
                spawner.spawnInterval = 0.5f;
                StartCoroutine(TriggerCatapultAttacks());
                yield return new WaitForSeconds(duration / 4);
                StartCoroutine(TriggerCatapultAttacks());
                yield return new WaitForSeconds(duration / 4);
                break;
            default:
                break;
        }
        EndWave();
    }


    private void SpawnCatapults()
    {
        if (catapults == null || catapults.Length == 0) return;
        foreach (GameObject cat in catapults)
        {
            cat.SetActive(true);
        }
    }
    private IEnumerator TriggerCatapultAttacks()
    {
        if (catapults == null || catapults.Length == 0) yield break;
        // Set a random catapult to target the player
        catapults[Random.Range(0, catapults.Length)].GetComponent<Catapult>().targetPlayer = true;
        // Trigger the attack function for each catapult
        foreach (GameObject cat in catapults)
        {
            if (cat != null && cat.activeSelf) cat.GetComponent<Catapult>().Attack();
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
    }


    private IEnumerator ChangeToDay()
    {
        if (waveNum > 1)
        {
            DNCycleManager.SetDay();
            yield return new WaitForSeconds(DNCycleManager.transitionDuration);            
        }

        _waveTime = 0;
        // Start coroutine to update wave settings
        waveUpdating = StartCoroutine(UpdateWaveOverTime(waveDuration));
        // Start enemy spawning coroutine
        enemySpawning = StartCoroutine(spawner.SpawnEnemies());
    }
    // Wait for transition,
    private IEnumerator ChangeToNight()
    {
        DNCycleManager.SetNight();
        yield return new WaitForSeconds(DNCycleManager.transitionDuration);
        KillRemainingGoblins();
        if (shop != null) shop.SetActive(true);
    }


    private void KillRemainingGoblins()
    {
        GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag("Goblin");
        foreach (GameObject enemy in spawnedEnemies)
        {
            EnemyVitals vitals = enemy.GetComponent<EnemyVitals>();
            if (vitals != null) vitals.OnGameObjectDestroyed(enemy);
        }
    }
}

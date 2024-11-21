using UnityEngine;
using UnityEngine.UI; // For Unity UI Text
using TMPro;          // Uncomment if using TextMeshPro

public class WaveController : MonoBehaviour
{
    // public Text timerText;  // For UnityEngine.UI.Text
    public TMP_Text timerText; // Uncomment if using TextMeshPro
    public int waveNum;
    public EnemySpawner spawner;

    private float _elapsedTime = 0;


    void Update()
    {
        // Increment the elapsed time
        _elapsedTime += Time.deltaTime;
        // Increase wave num every 30 seconds
        waveNum = Mathf.FloorToInt(_elapsedTime / 30);

        // Update functions
        UpdateTimerText();
        UpdateWave();
    }

    private void UpdateTimerText()
    {
        // Calculate minutes and seconds
        int displayMinutes = Mathf.FloorToInt(_elapsedTime / 60);
        int displaySeconds = Mathf.FloorToInt(_elapsedTime % 60);

        // Format the text
        timerText.text = string.Format("{0:00}:{1:00}<br><size=40%>wave {2}</size>", displayMinutes, displaySeconds, waveNum + 1);
    }


    private void UpdateWave()
    {
        switch (waveNum)
        {
            // 0:00
            case 0:
                spawner.spawnInterval = 3f;
                return;
            // 0:30
            case 1:
                spawner.spawnInterval = 2.5f;
                return;
            // 1:00
            case 2:
                spawner.spawnInterval = 2.25f;
                return;
            // 1:30
            case 3:
                spawner.spawnInterval = 2f;
                return;
            // 2:00
            case 4:
                spawner.spawnInterval = 1.75f;
                return;
            // 2:30
            case 5:
                spawner.spawnInterval = 1.5f;
                return;
            // 3:00
            case 6:
                spawner.spawnInterval = 1f;
                return;
        }
    }
}

using UnityEngine;
using UnityEngine.UI; // For Unity UI Text
using TMPro;          // Uncomment if using TextMeshPro

public class WaveController : MonoBehaviour
{
    // public Text timerText;  // For UnityEngine.UI.Text
    public TMP_Text timerText; // Uncomment if using TextMeshPro
    public int waveNum;

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
        timerText.text = string.Format("{0:00}:{1:00}", displayMinutes, displaySeconds);
    }


    private void UpdateWave()
    {
        switch (waveNum)
        {
            // 0:30
            case 0:
                return;
            // 1:00
            case 1:
                return;
            // 1:30
            case 2:
                return;
            // 2:00
            case 3:
                return;
            // 2:30
            case 4:
                return;
            // 3:00
            case 5:
                return;
        }
    }
}

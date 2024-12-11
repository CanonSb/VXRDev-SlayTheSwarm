using System.Collections;
using UnityEngine;

public class TestDayNightCycle : MonoBehaviour
{
    [SerializeField] private float intervalDuration = 10f; // Time in seconds between transitions (day to night and back)

    private DayNightCycleManager cycleManager; // Reference to the DayNightCycleManager
    private bool isDay = true; // Track whether it's currently day

    void Start()
    {
        // Get the DayNightCycleManager instance
        cycleManager = FindObjectOfType<DayNightCycleManager>();
        if (cycleManager == null)
        {
            Debug.LogError("DayNightCycleManager not found in the scene!");
            return;
        }

        // Start the repeating transition coroutine
        StartCoroutine(TransitionLoop());
    }

    private IEnumerator TransitionLoop()
    {
        while (true)
        {
            // Trigger the transition
            if (isDay)
            {
                cycleManager.SetNight();
            }
            else
            {
                cycleManager.SetDay();
            }

            // Toggle the current state
            isDay = !isDay;

            // Wait for the interval duration before switching again
            yield return new WaitForSeconds(intervalDuration);
        }
    }
}
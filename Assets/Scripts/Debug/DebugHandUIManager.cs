using UnityEngine;
using TMPro; // For TextMeshPro support

public class DebugHandUIManager : MonoBehaviour
{
    public TextMeshProUGUI leftHandText; // Reference to Left Hand Text UI
    public TextMeshProUGUI rightHandText; // Reference to Right Hand Text UI

    public TextMeshProUGUI statusText; // Reference to Status Text UI
    public PlayerHandManager handManager; // Reference to the PlayerHandManager script

    private void Update()
    {
        if (handManager != null)
        {
            // Update Left Hand Text
            var leftItem = handManager.GetItemInLeftHand();
            leftHandText.text = leftItem != null
                ? $"Left Hand: {leftItem.itemData.itemName}"
                : "Left Hand: Empty";

            // Update Right Hand Text
            var rightItem = handManager.GetItemInRightHand();
            rightHandText.text = rightItem != null
                ? $"Right Hand: {rightItem.itemData.itemName}"
                : "Right Hand: Empty";


            // Display current time in status text
            statusText.text = handManager.lastDebugLogMessage;
        }
    }
}
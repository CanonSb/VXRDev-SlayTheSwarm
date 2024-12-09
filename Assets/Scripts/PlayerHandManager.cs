using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHandManager : MonoBehaviour
{
    public XRDirectInteractor leftHandInteractor;  // Reference to left hand interactor
    public XRDirectInteractor rightHandInteractor; // Reference to right hand interactor

    public XRRayInteractor leftHandRayInteractor;  // Optional, for distant grab
    public XRRayInteractor rightHandRayInteractor; // Optional, for distant grab

    public Transform leftHandStabilizedController;  // Stabilized controller for the left hand
    public Transform rightHandStabilizedController; // Stabilized controller for the right hand

    private EquippableItem leftHandItem;
    private EquippableItem rightHandItem;

    // Store last debug log message as string
    public string lastDebugLogMessage = "No debug message";


    private void OnEnable()
    {
        // Direct interactors
        if (leftHandInteractor != null)
        {
            leftHandInteractor.selectEntered.AddListener(OnLeftHandSelect);
            leftHandInteractor.selectExited.AddListener(OnLeftHandDeselect);
        }
        if (rightHandInteractor != null)
        {
            rightHandInteractor.selectEntered.AddListener(OnRightHandSelect);
            rightHandInteractor.selectExited.AddListener(OnRightHandDeselect);
        }

        // Ray interactors
        if (leftHandRayInteractor != null)
        {
            leftHandRayInteractor.selectEntered.AddListener(OnLeftHandSelect);
            leftHandRayInteractor.selectExited.AddListener(OnLeftHandDeselect);
        }
        if (rightHandRayInteractor != null)
        {
            rightHandRayInteractor.selectEntered.AddListener(OnRightHandSelect);
            rightHandRayInteractor.selectExited.AddListener(OnRightHandDeselect);
        }
    }

    private void OnDisable()
    {
        // Direct interactors
        if (leftHandInteractor != null)
        {
            leftHandInteractor.selectEntered.RemoveListener(OnLeftHandSelect);
            leftHandInteractor.selectExited.RemoveListener(OnLeftHandDeselect);
        }
        if (rightHandInteractor != null)
        {
            rightHandInteractor.selectEntered.RemoveListener(OnRightHandSelect);
            rightHandInteractor.selectExited.RemoveListener(OnRightHandDeselect);
        }

        // Ray interactors
        if (leftHandRayInteractor != null)
        {
            leftHandRayInteractor.selectEntered.RemoveListener(OnLeftHandSelect);
            leftHandRayInteractor.selectExited.RemoveListener(OnLeftHandDeselect);
        }
        if (rightHandRayInteractor != null)
        {
            rightHandRayInteractor.selectEntered.RemoveListener(OnRightHandSelect);
            rightHandRayInteractor.selectExited.RemoveListener(OnRightHandDeselect);
        }
    }
    private void OnLeftHandSelect(SelectEnterEventArgs args)
    {
        leftHandItem = args.interactableObject.transform.GetComponent<EquippableItem>();
        if (leftHandItem != null)
        {
            Debug.Log($"Left hand equipped: {leftHandItem.itemData.itemName}");
            // Attach the item to the stabilized controller
            leftHandItem.transform.SetParent(leftHandStabilizedController, false);
            leftHandItem.transform.localPosition = Vector3.zero;  // Optional: Reset position
            leftHandItem.transform.localRotation = Quaternion.identity;  // Optional: Reset rotation
        }
    }

    private void OnRightHandSelect(SelectEnterEventArgs args)
    {
        rightHandItem = args.interactableObject.transform.GetComponent<EquippableItem>();
        if (rightHandItem != null)
        {
            Debug.Log($"Right hand equipped: {rightHandItem.itemData.itemName}");
            // Attach the item to the stabilized controller
            rightHandItem.transform.SetParent(rightHandStabilizedController, false);
            rightHandItem.transform.localPosition = Vector3.zero;  // Optional: Reset position
            rightHandItem.transform.localRotation = Quaternion.identity;  // Optional: Reset rotation
        }
    }

    private void OnLeftHandDeselect(SelectExitEventArgs args)
    {
        if (leftHandItem != null)
        {
            Debug.Log($"Left hand unequipped: {leftHandItem.itemData.itemName}");
            // Detach the item
            leftHandItem.transform.SetParent(null);
            leftHandItem = null;
        }
    }

    private void OnRightHandDeselect(SelectExitEventArgs args)
    {
        if (rightHandItem != null)
        {
            Debug.Log($"Right hand unequipped: {rightHandItem.itemData.itemName}");
            // Detach the item
            rightHandItem.transform.SetParent(null);
            rightHandItem = null;
        }
    }

    public EquippableItem GetItemInLeftHand()
    {
        return leftHandItem;
    }

    public EquippableItem GetItemInRightHand()
    {
        return rightHandItem;
    }
}
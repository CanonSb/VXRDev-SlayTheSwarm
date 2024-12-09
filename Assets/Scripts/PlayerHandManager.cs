using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHandManager : MonoBehaviour
{
    public XRDirectInteractor leftHandInteractor;  // Reference to left hand interactor
    public XRDirectInteractor rightHandInteractor; // Reference to right hand interactor

    private EquippableItem leftHandItem;
    private EquippableItem rightHandItem;

    private void OnEnable()
    {
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
    }

    private void OnDisable()
    {
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
    }

    private void OnLeftHandSelect(SelectEnterEventArgs args)
    {
        leftHandItem = args.interactableObject.transform.GetComponent<EquippableItem>();
        if (leftHandItem != null)
        {
            Debug.Log($"Left hand equipped: {leftHandItem.itemData.itemName}");
        }
    }

    private void OnLeftHandDeselect(SelectExitEventArgs args)
    {
        if (leftHandItem != null)
        {
            Debug.Log($"Left hand unequipped: {leftHandItem.itemData.itemName}");
            leftHandItem = null;
        }
    }

    private void OnRightHandSelect(SelectEnterEventArgs args)
    {
        rightHandItem = args.interactableObject.transform.GetComponent<EquippableItem>();
        if (rightHandItem != null)
        {
            Debug.Log($"Right hand equipped: {rightHandItem.itemData.itemName}");
        }
    }

    private void OnRightHandDeselect(SelectExitEventArgs args)
    {
        if (rightHandItem != null)
        {
            Debug.Log($"Right hand unequipped: {rightHandItem.itemData.itemName}");
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
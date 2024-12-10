using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHandManager : MonoBehaviour
{
    // Interactors
    public XRDirectInteractor leftHandInteractor;  // Direct interactor for left hand
    public XRDirectInteractor rightHandInteractor; // Direct interactor for right hand
    public XRRayInteractor leftHandRayInteractor;  // Ray interactor for left hand
    public XRRayInteractor rightHandRayInteractor; // Ray interactor for right hand

    // Stabilized controllers (grip points)
    public Transform leftHandStabilizedController;
    public Transform rightHandStabilizedController;

    // Hand Models
    public GameObject openHandModel_Left;
    public GameObject grippedHandModel_Left;
    public GameObject openHandModel_Right;
    public GameObject grippedHandModel_Right;

    // public Transform attachPoint_LeftOpen;  // Attach point for the left open hand
    // public Transform attachPoint_LeftGripped;  // Attach point for the left gripped hand
    // public Transform attachPoint_RightOpen;  // Attach point for the right open hand
    // public Transform attachPoint_RightGripped;  // Attach point for the right gripped hand

    // Equipped Items
    private EquippableItem leftHandItem;
    private EquippableItem rightHandItem;

    // Store last debug log message
    public string lastDebugLogMessage = "No debug message";

    private void Start()
    {
        // Ensure open hands are active and gripped hands are inactive at the start
        openHandModel_Left.SetActive(true);
        grippedHandModel_Left.SetActive(false);
        openHandModel_Right.SetActive(true);
        grippedHandModel_Right.SetActive(false);
    }

    private void OnEnable()
    {
        // Subscribe to Direct Interactor events
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

        // Subscribe to Ray Interactor events
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
        // Unsubscribe from Direct Interactor events
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

        // Unsubscribe from Ray Interactor events
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
            lastDebugLogMessage = $"Left hand equipped: {leftHandItem.itemData.itemName}";

            // Attach the item to the stabilized controller
            leftHandItem.transform.SetParent(leftHandStabilizedController, false);

            XRGrabInteractable grabInteractable = leftHandItem.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.attachTransform != null)
            {
                leftHandItem.transform.localPosition = grabInteractable.attachTransform.localPosition;
                leftHandItem.transform.localRotation = grabInteractable.attachTransform.localRotation;
            }

            // Switch to gripped hand model
            SetHandModelState(true, true);
        }
    }

    private void OnRightHandSelect(SelectEnterEventArgs args)
    {
        rightHandItem = args.interactableObject.transform.GetComponent<EquippableItem>();
        if (rightHandItem != null)
        {
            Debug.Log($"Right hand equipped: {rightHandItem.itemData.itemName}");
            lastDebugLogMessage = $"Right hand equipped: {rightHandItem.itemData.itemName}";

            // Attach the item to the stabilized controller
            rightHandItem.transform.SetParent(rightHandStabilizedController, false);

            XRGrabInteractable grabInteractable = rightHandItem.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.attachTransform != null)
            {
                rightHandItem.transform.localPosition = grabInteractable.attachTransform.localPosition;
                rightHandItem.transform.localRotation = grabInteractable.attachTransform.localRotation;
            }

            // Switch to gripped hand model
            SetHandModelState(false, true);
        }
    }

    private void OnLeftHandDeselect(SelectExitEventArgs args)
    {
        if (leftHandItem != null)
        {
            Debug.Log($"Left hand unequipped: {leftHandItem.itemData.itemName}");
            lastDebugLogMessage = $"Left hand unequipped: {leftHandItem.itemData.itemName}";

            // Detach the item
            leftHandItem.transform.SetParent(null);
            leftHandItem = null;

            // Switch to open hand model
            SetHandModelState(true, false);
        }
    }

    private void OnRightHandDeselect(SelectExitEventArgs args)
    {
        if (rightHandItem != null)
        {
            Debug.Log($"Right hand unequipped: {rightHandItem.itemData.itemName}");
            lastDebugLogMessage = $"Right hand unequipped: {rightHandItem.itemData.itemName}";

            // Detach the item
            rightHandItem.transform.SetParent(null);
            rightHandItem = null;

            // Switch to open hand model
            SetHandModelState(false, false);
        }
    }

    private void SetHandModelState(bool isLeftHand, bool isGripped)
    {
        // Determine the active hand model
        GameObject activeHandModel = isLeftHand
            ? (isGripped ? grippedHandModel_Left : openHandModel_Left)
            : (isGripped ? grippedHandModel_Right : openHandModel_Right);

        // Activate the appropriate hand model
        if (isLeftHand)
        {
            openHandModel_Left.SetActive(!isGripped);
            grippedHandModel_Left.SetActive(isGripped);
        }
        else
        {
            openHandModel_Right.SetActive(!isGripped);
            grippedHandModel_Right.SetActive(isGripped);
        }

        // Debugging output
        lastDebugLogMessage = $"Active Model: {(isLeftHand ? (isGripped ? "Left Gripped" : "Left Open") : (isGripped ? "Right Gripped" : "Right Open"))}";
    }

    private Transform GetAttachPoint(GameObject handModel)
    {
        // Search for the direct child named "Attach Point"
        Transform attachPoint = handModel.transform.Find("Attach Point");
        if (attachPoint == null)
        {
            Debug.LogError($"Attach Point not found in '{handModel.name}'!");
        }
        return attachPoint;
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
using UnityEngine;

public class EquippableItem : MonoBehaviour
{
    public EquippableItemData itemData; // Reference to the ScriptableObject

    private void Start()
    {
        ValidateSetup();
    }

    // Validate that the prefab is correctly set up
    private void ValidateSetup()
    {
        if (itemData == null)
        {
            Debug.LogError($"ItemData not assigned for {gameObject.name}. Please assign it in the Inspector.");
        }

        // Check for a Collider on this GameObject or its children
        Collider collider = GetComponentInChildren<Collider>();
        if (collider == null)
        {
            Debug.LogError($"No Collider found on {gameObject.name}. Please add a Collider to the prefab.");
        }
        else
        {
            Debug.Log($"Setup validated for {gameObject.name}.");
        }
    }

    public void Equip()
    {
        if (itemData != null)
        {
            Debug.Log($"{itemData.itemName} equipped.");
        }
    }

    public void Unequip()
    {
        if (itemData != null)
        {
            Debug.Log($"{itemData.itemName} unequipped.");
        }
    }
}
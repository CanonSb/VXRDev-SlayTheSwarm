using UnityEngine;

[CreateAssetMenu(fileName = "NewEquippableItem", menuName = "Items/Equippable Item")]
public class EquippableItemData : ScriptableObject
{
    public string itemName;
    public string description; // Developer-friendly desc. of item
    public bool allowTeleportation; // Can the player teleport with this item?
    public bool allowBlocking; // Can the player block with this item?
    public bool allowDamage; // Can the player deal damage with this item?
    public int damageMultiplier = -1; // How much damage does this item deal?
}
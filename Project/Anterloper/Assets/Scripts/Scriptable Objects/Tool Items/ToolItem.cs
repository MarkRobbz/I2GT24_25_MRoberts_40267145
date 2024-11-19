using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    None,
    Axe,
    Pickaxe,
}

[CreateAssetMenu(fileName = "New Tool Item", menuName = "Items/Tool Item")]
public class ToolItem : CraftableItem
{
    public ToolType toolType;
    public float baseDamage = 10f; // Base damage (optional)

    // Define a list of damage values against different target types
    [System.Serializable]
    public struct DamageAgainstTarget
    {
        public TargetType targetType;
        public float damageValue;
    }

    public List<DamageAgainstTarget> damageValues;

    // Method to get damage against a specific target type
    public float GetDamageAgainst(TargetType targetType)
    {
        foreach (var damageEntry in damageValues)
        {
            if (damageEntry.targetType == targetType)
            {
                return damageEntry.damageValue;
            }
        }
        return 0f; // Default to 0 if no entry found
    }

    public void UseTool()
    {
        Debug.Log($"Using {itemName} as a {toolType}");

        // Get the player's equipped item model
        PlayerEquipment playerEquipment = FindObjectOfType<PlayerEquipment>();
        GameObject equippedItemModel = playerEquipment?.equippedItemModel;

        if (equippedItemModel != null)
        {
            // Try to get the Attack component
            Attack attackComponent = equippedItemModel.GetComponent<Attack>();
            if (attackComponent == null)
            {
                attackComponent = equippedItemModel.AddComponent<Attack>();
                attackComponent.attackRange = 3f; // Adjust as needed
                attackComponent.targetLayer = LayerMask.GetMask("Targets", "NodeLayer");
            }

            // Set the tool item in the Attack component
            attackComponent.toolItem = this;

            // Perform the attack
            attackComponent.PerformAttack();
        }
        else
        {
            Debug.LogWarning("No equipped item model found.");
        }
    }
}
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
    public float toolDamage = 25f; 
   
   

    public void UseTool()
    {
        Debug.Log($"Using {itemName} as a {toolType}");

        // Get players equipped item model
        PlayerEquipment playerEquipment = FindObjectOfType<PlayerEquipment>();
        GameObject equippedItemModel = playerEquipment?.equippedItemModel;

        if (equippedItemModel != null)
        {
            // Try get the Attack component
            Attack attackComponent = equippedItemModel.GetComponent<Attack>();
            if (attackComponent == null)
            {
                attackComponent = equippedItemModel.AddComponent<Attack>();
                attackComponent.damage = toolDamage;
                attackComponent.attackRange = 3f; 
                attackComponent.targetLayer = LayerMask.GetMask("Targets", "NodeLayer");
            }

            // Perform attack
            attackComponent.PerformAttack();
        }
        else
        {
            Debug.LogWarning("No equipped item model found.");
        }
    }



}

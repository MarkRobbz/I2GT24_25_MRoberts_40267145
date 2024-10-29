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

    public void UseTool()
    {
        
        Debug.Log($"Using {itemName} as a {toolType}");
    }

    
    public override void Use(bool isHold)
    {
        PlayerEquipment playerEquipment = FindObjectOfType<PlayerEquipment>();
        if (playerEquipment != null)
        {
            playerEquipment.EquipItem(this);
        }
        else
        {
            Debug.LogError("PlayerEquipment not found in the scene.");
        }
    }
}



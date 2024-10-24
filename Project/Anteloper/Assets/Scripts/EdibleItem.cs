using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewEdibleItem", menuName = "Inventory/EdibleItem")]
public class EdibleItem : ConsumableItem
{
    public override void Consume()
    {
        HungerAndThirst playerStats = FindObjectOfType<HungerAndThirst>();
        if (playerStats != null)
        {
            playerStats.ModifyHunger(restoreAmount);
            Debug.Log($"Consumed {itemName}, restored {restoreAmount} hunger.");
        }
        else
        {
            Debug.LogWarning("PlayerStats not found in scene.");
        }
    }
}

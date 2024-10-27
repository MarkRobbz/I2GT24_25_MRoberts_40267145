using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewDrinkableItem", menuName = "Inventory/DrinkableItem")]
public class DrinkableItem : ConsumableItem
{
    public override void Consume()
    {
        HungerAndThirst playerStats = FindObjectOfType<HungerAndThirst>();
        if (playerStats != null)
        {
            playerStats.ModifyThirst(restoreAmount);
            Debug.Log($"Consumed {itemName}, restored {restoreAmount} thirst.");
        }
        else
        {
            Debug.LogError("HungerAndThirst component not found.");
        }
    }
}



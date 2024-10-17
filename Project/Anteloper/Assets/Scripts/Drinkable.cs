using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drinkable : Item
{
    //private Health _playerHealth;
    private int restoreAmount;
    
    

    public override void Use()
    {
        HungerAndThirst playerHunger = FindObjectOfType<HungerAndThirst>();
        if (playerHunger != null)
        {
            playerHunger.RestoreThirst(restoreAmount);
        }
        
        //DestroyItem();
    }
}

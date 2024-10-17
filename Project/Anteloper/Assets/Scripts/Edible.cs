using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : Item
{
    //private Health _playerHealth;
    private int restoreAmount;
    public FoodType foodType; 
    
    public enum FoodType
    {
        Sap,
        Mushroom
    }
    
    void Start()
    {
        //_playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        
        switch (foodType)
        {
            case FoodType.Sap:
                restoreAmount = 10;
                itemName = "Sap";
                break;
            case FoodType.Mushroom:
                restoreAmount = 20;
                itemName = "Mushroom";
                break;
            default:
                restoreAmount = 0;
                itemName = "Unknown Food";
                break;
        }
    }

    public override void Use()
    {
        HungerAndThirst playerHunger = FindObjectOfType<HungerAndThirst>();
        if (playerHunger != null)
        {
            playerHunger.RestoreHunger(restoreAmount);
        }
        
        DestroyItem();
    }
}

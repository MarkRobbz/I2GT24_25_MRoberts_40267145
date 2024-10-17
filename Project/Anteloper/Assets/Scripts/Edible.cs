using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : Item
{
    //private Health _playerHealth;
    private float _restoreAmount;
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
                _restoreAmount = 10;
                itemName = "Sap";
                break;
            case FoodType.Mushroom:
                _restoreAmount = 20;
                itemName = "Mushroom";
                break;
            default:
                _restoreAmount = 0;
                itemName = "Unknown Food";
                break;
        }
    }

    public override void Use()
    {
        HungerAndThirst playerHunger = FindObjectOfType<HungerAndThirst>();
        if (playerHunger != null)
        {
            playerHunger.RestoreHunger(_restoreAmount);
        }
        
        DestroyItem();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerAndThirst : MonoBehaviour
{
   [SerializeField] private float _currentHunger;
   [SerializeField] private float _maxHunger;
   [SerializeField] private float _currentThirst;
   [SerializeField] private float _maxThirst;
   
   //Need to add events for UI triggers;



   public void RestoreHunger(float hungerAmount)
   {
       _currentHunger = Math.Clamp(_currentHunger + hungerAmount, 0, _maxHunger);
   }
   
   public void RestoreThirst(float thirstAmount)
   {
       _currentHunger = Math.Clamp(_currentHunger + thirstAmount, 0, _maxHunger);
   }
   
   
}

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
   
   
    public event Action<float> onHungerChanged; 
    public event Action<float> onThirstChanged;

    public float CurrentHunger
    { 
        get { return _currentHunger; }
        private set
        {
        _currentHunger = Mathf.Clamp(value, 0, _maxHunger);
        UpdateHungerUI();
        }
    }
    
    public float MaxHunger
    { 
        get { return _maxHunger; }
        set
        {
            if (value > 0)
            {
                _maxHunger = value;
                UpdateHungerUI();
            }
        }
    }
    
    public float CurrentThirst
    { 
        get { return _currentThirst; }
        private set
        {
            _currentThirst = Mathf.Clamp(value, 0, _maxThirst);
            UpdateThirstUI();
        }
    }
    
    public float MaxThirst
    { 
        get { return _maxThirst; }
        set
        {
            if (value > 0)
            {
                _maxThirst = value;
                UpdateThirstUI();
            }
        }
    }

   public void RestoreHunger(float hungerAmount)
   {
       _currentHunger = Math.Clamp(_currentHunger + hungerAmount, 0, _maxHunger);
       UpdateHungerUI();
   }
   
   public void RestoreThirst(float thirstAmount)
   {
       _currentThirst = Math.Clamp(_currentHunger + thirstAmount, 0, _maxHunger);
       UpdateThirstUI();
   }

   public void UpdateHungerUI()
   {
       onHungerChanged?.Invoke(_currentHunger);
   }

   public void UpdateThirstUI()
   {
       onThirstChanged?.Invoke(_currentThirst);
   }
}

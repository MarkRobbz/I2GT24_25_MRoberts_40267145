using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
   
   public void AbolishHunger(float hungerAmount)
   {
       _currentHunger = Math.Clamp(_currentHunger -= hungerAmount, 0, _maxHunger);
       UpdateHungerUI();
   }
   
   public void RestoreThirst(float thirstAmount)
   {
       _currentThirst = Math.Clamp(_currentThirst + thirstAmount, 0, _maxThirst);
       UpdateThirstUI();
   }
   
   public void AbolishThirst(float thirstAmount)
   {
       _currentThirst = Math.Clamp(_currentThirst -= thirstAmount, 0, _maxThirst);
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
   
   //============ Test methods Inspector =============

   [Space(20)]
   [VerticalGroup("HungerGroup")]
   [ShowInInspector]
   private float hungerTestAmount = 10f; 
   [HorizontalGroup("HungerGroup/Buttons", Width = 150)]
   [Button("Restore Hunger (Test)"), GUIColor(0.3f, 0.9f, 0.3f)] 
   private void TestRestoreHunger()
   {
       RestoreHunger(hungerTestAmount);
       Debug.Log("Test-Hunger increased by: " + hungerTestAmount);
   }

   [HorizontalGroup("HungerGroup/Buttons", Width = 150)]
   [Button("Decrease Hunger (Test)"), GUIColor(0.9f, 0.3f, 0.3f)] 
   private void TestAbolishHunger()
   {
       AbolishHunger(hungerTestAmount);
       Debug.Log("Test-Hunger decreased by: " + hungerTestAmount);
   }


   [Space(20)]


   [VerticalGroup("ThirstGroup")]
   [ShowInInspector]
   private float thirstTestAmount = 10f; 


   [HorizontalGroup("ThirstGroup/Buttons", Width = 150)]
   [Button("Restore Thirst (Test)"), GUIColor(0.3f, 0.9f, 0.3f)] 
   private void TestRestoreThirst()
   {
       RestoreThirst(thirstTestAmount);
       Debug.Log("Test-Thirst increased by: " + thirstTestAmount);
   }

   [HorizontalGroup("ThirstGroup/Buttons", Width = 150)]
   [Button("Decrease Thirst (Test)"), GUIColor(0.9f, 0.3f, 0.3f)] 
   private void TestAbolishThirst()
   {
       AbolishThirst(thirstTestAmount);
       Debug.Log("Test-Thirst decreased by: " + thirstTestAmount);
   }

}

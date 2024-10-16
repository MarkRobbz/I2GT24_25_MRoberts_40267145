using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Health : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth;

    public event Action<float> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth);
        
    }

    
    public void IncreaseHealth(float healthPoints)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthPoints;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    public void DecreaseHealth(float healthPoints)
    {
        if (healthPoints >= currentHealth)
        {
            Die();
        }
        else
        {
            currentHealth -= healthPoints;
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    private void Die()
    {
        currentHealth = 0;
        OnHealthChanged?.Invoke(currentHealth);
        Debug.Log("DEAD! Current Health: " + currentHealth);
    }

    
    
    
    
    
    
    //============ Test methods for the Inspector =============

    [Space(20)]
    [ShowInInspector]
    private float healthTestAmount = 10f; 

    // Group the buttons horizontally
    [HorizontalGroup("HealthButtons", Width = 100)] 
    [Button("Increase Health (Test)"), GUIColor(0.3f, 0.9f, 0.3f)] 
    private void TestIncreaseHealth()
    {
        IncreaseHealth(healthTestAmount);
        Debug.Log("Health increased by: " + healthTestAmount);
    }

    [HorizontalGroup("HealthButtons", Width = 100)] 
    [Button("Decrease Health (Test)"), GUIColor(0.9f, 0.3f, 0.3f)] 
    private void TestDecreaseHealth()
    {
        DecreaseHealth(healthTestAmount);
        Debug.Log("Health decreased by: " + healthTestAmount);
    }
}
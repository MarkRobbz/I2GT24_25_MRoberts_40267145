using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Health : MonoBehaviour, IAttackable
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private TargetType _targetType = TargetType.None; // *Set in Inspector*

    public event Action<float> OnHealthChanged;
    
    
    public float CurrentHealth
    {
        get { return _currentHealth; }
        private set
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
            UpdateHealthUI();
        }
    }
    public float MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            if (value > 0)
            {
                _maxHealth = value;
                UpdateHealthUI();
            }
        }
    }

    
    private void Start()
    {
        UpdateHealthUI();
    }

    
    public void IncreaseHealth(float healthPoints)
    {
            _currentHealth = Mathf.Clamp(_currentHealth + healthPoints, 0, _maxHealth);
            UpdateHealthUI();
        
    }
    

    public void DecreaseHealth(float healthPoints)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - healthPoints, 0, _maxHealth);
        UpdateHealthUI();
        
        if ( _currentHealth <= 0)
        {
            Die();
        }
        
    }
    
    public void TakeDamage(float damage)
    {
        DecreaseHealth(damage);
    }
    
    public void ChangeHealthOverTime(float healthPoints, float duration, bool isIncrease)
    {
        StartCoroutine(ChangeHealthCoroutine(healthPoints, duration, isIncrease));
    }

    
    private IEnumerator ChangeHealthCoroutine(float healthPoints, float duration, bool isIncrease)
    {
        float elapsedTime = 0f;
        float amountPerSecond = healthPoints / duration;

        while (elapsedTime < duration)
        {
            float deltaTime = Time.deltaTime;
            float healthChange = amountPerSecond * deltaTime;

            if (isIncrease)
            {
                IncreaseHealth(healthChange); 
            }
            else
            {
                DecreaseHealth(healthChange); 
            }

            elapsedTime += deltaTime;
            yield return null;
        }

        
        if (isIncrease)
        {
            IncreaseHealth(healthPoints); 
        }
        else
        {
            DecreaseHealth(healthPoints); 
        }
    }


    private void Die()
    {
        _currentHealth = 0;
        UpdateHealthUI();
        Debug.Log("DEAD! Current Health: " + _currentHealth);
    }

    public void UpdateHealthUI()
    {
        OnHealthChanged?.Invoke(_currentHealth);
    }

    
    
    public TargetType GetTargetType()
    {
        return _targetType;
    }
    
    
    
    //============ Test methods Inspector =============

    [Space(20)]
    [ShowInInspector]
    private float healthTestAmount = 10f; 

    // Group the buttons horizontally
    [HorizontalGroup("HealthButtons", Width = 100)] 
    [Button("Increase Health (Test)"), GUIColor(0.3f, 0.9f, 0.3f)] 
    private void TestIncreaseHealth()
    {
        IncreaseHealth(healthTestAmount);
        Debug.Log("Test-Health increased by: " + healthTestAmount);
    }

    [HorizontalGroup("HealthButtons", Width = 100)] 
    [Button("Decrease Health (Test)"), GUIColor(0.9f, 0.3f, 0.3f)] 
    private void TestDecreaseHealth()
    {
        DecreaseHealth(healthTestAmount);
        Debug.Log("Test-Health decreased by: " + healthTestAmount);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float _currentHealth;
    [SerializeField] public float maxHealth;

    public event Action<float> OnHealthChanged;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DecreaseHealth(10);
        }
    }

    private void IncreaseHealth(float healthPoints)
    {
        if (_currentHealth < maxHealth)
        {
            if (healthPoints > maxHealth)
            {
                _currentHealth = maxHealth;
            }
            _currentHealth = healthPoints;
        }
    }

    private void DecreaseHealth(float healthPoints)
    {
        if (healthPoints >= _currentHealth)
        {
            Die();
        }
        else
        {
            _currentHealth -= healthPoints;
            OnHealthChanged.Invoke(_currentHealth);
        }
    }

    void Die()
    {
        _currentHealth = 0;
        Debug.Log("Dead! Current Health: " + _currentHealth);
    }
}

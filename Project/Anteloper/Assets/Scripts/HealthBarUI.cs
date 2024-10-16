using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] private Health _playerHealth;


    private void Start()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        _healthBarSlider = gameObject.GetComponentInChildren<Slider>();
        _healthBarSlider.maxValue = _playerHealth.maxHealth;
        _healthBarSlider.minValue = 0;
        UpdateHealthBar(_playerHealth._currentHealth);
        
        _playerHealth.OnHealthChanged += UpdateHealthBar; //Sub

    }

    private void UpdateHealthBar(float currentHealth)
    {
        _healthBarSlider.value = currentHealth;
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}

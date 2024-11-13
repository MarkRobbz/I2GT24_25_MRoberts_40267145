using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstBarUI : MonoBehaviour
{
    [SerializeField] private Slider _thirstBarSlider;
    [SerializeField] private HungerAndThirst _playerThirst;

    private void Start()
    {
        _playerThirst = GameObject.FindGameObjectWithTag("Player").GetComponent<HungerAndThirst>();
        _thirstBarSlider = gameObject.GetComponentInChildren<Slider>();
        _thirstBarSlider.minValue = 0;
        _thirstBarSlider.maxValue = _playerThirst.MaxThirst;
        UpdateThirstBar(_playerThirst.CurrentThirst); 
        
        _playerThirst.onThirstChanged += UpdateThirstBar;
        
        
    }

    private void UpdateThirstBar(float currentThirst)
    {
        _thirstBarSlider.value = _playerThirst.CurrentThirst;
    }
    
    private void OnDestroy()
    {
        if (_playerThirst != null)
        {
            _playerThirst.onThirstChanged -= UpdateThirstBar;
        }
    }
}

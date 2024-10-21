using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBarUI : MonoBehaviour
{
    
    [SerializeField] private Slider _hungerBarSlider;
    [SerializeField] private HungerAndThirst _playerHunger;

    private void Start()
    {
        _playerHunger = GameObject.FindGameObjectWithTag("Player").GetComponent<HungerAndThirst>();
        _hungerBarSlider= gameObject.GetComponentInChildren<Slider>();
        _hungerBarSlider.minValue = 0;
        _hungerBarSlider.maxValue = _playerHunger.MaxHunger;
        UpdateHungerBar(_playerHunger.CurrentHunger); 
        
        _playerHunger.onHungerChanged += UpdateHungerBar;
        
        
    }

    private void UpdateHungerBar(float currentThirst)
    {
        _hungerBarSlider.value = _playerHunger.CurrentHunger;
    }
    
    private void OnDestroy()
    {
        if (_playerHunger != null)
        {
            _playerHunger.onHungerChanged -= UpdateHungerBar;
        }
    }
}



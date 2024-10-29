using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class HungerAndThirst : MonoBehaviour
{
    [Header("Hunger Settings")]
    [SerializeField] private float _currentHunger = 100f;
    [SerializeField] private float _maxHunger = 100f;
    [SerializeField] private float _hungerDecreaseRate = 0.1f; // per second

    [Header("Thirst Settings")]
    [SerializeField] private float _currentThirst = 100f;
    [SerializeField] private float _maxThirst = 100f;
    [SerializeField] private float _thirstDecreaseRate = 0.1f; // per second

    [Header("Health Decrease Due to Hunger or Thirst")]
    [SerializeField] private float _healthDecreaseRate = 0.5f; // per second

    public event Action<float> onHungerChanged;
    public event Action<float> onThirstChanged;

    private Health _playerHealth;

    
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

    private void Start()
    {
        _playerHealth = GetComponent<Health>();
        if (_playerHealth == null)
        {
            Debug.LogError("Health component not found on the player.");
        }
        
        UpdateHungerUI();
        UpdateThirstUI();
    }

    private void Update()
    {
        DecreaseHungerAndThirst();

        if (CurrentHunger <= 0 || CurrentThirst <= 0)
        {
            ReduceHealthDueToHungerOrThirst();
        }
    }

    private void DecreaseHungerAndThirst()
    {
        float hungerDecreaseAmount = _hungerDecreaseRate * Time.deltaTime;
        float thirstDecreaseAmount = _thirstDecreaseRate * Time.deltaTime;

        ModifyHunger(-hungerDecreaseAmount);
        ModifyThirst(-thirstDecreaseAmount);
    }

    private void ReduceHealthDueToHungerOrThirst()
    {
        if (_playerHealth == null) return;

        
        if (CurrentHunger > 0 && CurrentThirst > 0)
        {
            
            return;
        }

        float minHealth = _playerHealth.MaxHealth * 0.1f; // 10% of max health

        if (_playerHealth.CurrentHealth > minHealth)
        {
            float healthDecreaseAmount = _healthDecreaseRate * Time.deltaTime;
            float potentialNewHealth = _playerHealth.CurrentHealth - healthDecreaseAmount;

            
            if (potentialNewHealth < minHealth) //Doesn't drop below 10
            {
                healthDecreaseAmount = _playerHealth.CurrentHealth - minHealth;
            }

            _playerHealth.DecreaseHealth(healthDecreaseAmount);
        }
    }

    
    public void ModifyHunger(float amount)
    {
        CurrentHunger += amount;
    }

    public void ModifyThirst(float amount)
    {
        CurrentThirst += amount;
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
        ModifyHunger(hungerTestAmount);
        Debug.Log("Test-Hunger increased by: " + hungerTestAmount);
    }

    [HorizontalGroup("HungerGroup/Buttons", Width = 150)]
    [Button("Decrease Hunger (Test)"), GUIColor(0.9f, 0.3f, 0.3f)]
    private void TestReduceHunger()
    {
        ModifyHunger(-hungerTestAmount);
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
        ModifyThirst(thirstTestAmount);
        Debug.Log("Test-Thirst increased by: " + thirstTestAmount);
    }

    [HorizontalGroup("ThirstGroup/Buttons", Width = 150)]
    [Button("Decrease Thirst (Test)"), GUIColor(0.9f, 0.3f, 0.3f)]
    private void TestReduceThirst()
    {
        ModifyThirst(-thirstTestAmount);
        Debug.Log("Test-Thirst decreased by: " + thirstTestAmount);
    }
}

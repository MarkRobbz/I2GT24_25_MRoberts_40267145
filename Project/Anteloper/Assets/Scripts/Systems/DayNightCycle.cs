using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public event Action OnDayStart;
    public event Action OnNightStart;
    public event Action OnNewDay;

    [SerializeField] private float dayLengthInMinutes = 1f; 
    private float _currentTime;
    private bool _isDaytime = true;
    private int _daysPassed = 0;

    private void Start()
    {
        _currentTime = 0f; //midnight
        Debug.Log("DayNightCycle is active");
    }

    private void Update()
    {
        UpdateTime();
        
    }

    private void UpdateTime()
    {
        _currentTime += Time.deltaTime * (24f / (dayLengthInMinutes * 60f)); 
        Debug.Log($"Current Time: {_currentTime}, Is Daytime: {_isDaytime}");

        if (_currentTime >= 24f) // A new day
        {
            _currentTime = 0f;
            _daysPassed++;
            OnNewDay?.Invoke();
            Debug.Log("New day started");
            if (!_isDaytime) StartDay();
        }
        else if (_isDaytime && _currentTime >= 18f) // 6 PM
        {
            Debug.Log("Transition to Night");
            StartNight();
        }
        else if (!_isDaytime && _currentTime >= 6f && _currentTime < 18f) // 6 AM
        {
            Debug.Log("Transition to Day");
            StartDay();
        }
    }


    private void StartDay()
    {
        _isDaytime = true;
        OnDayStart?.Invoke();
        Debug.Log("Day started.");
    }

    private void StartNight()
    {
        _isDaytime = false;
        OnNightStart?.Invoke();
        Debug.Log("Night started.");
    }

    public int GetDaysPassed() => _daysPassed;
    public bool IsDaytime() => _isDaytime;
}
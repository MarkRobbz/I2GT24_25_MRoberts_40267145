using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public event Action OnDayStart;
    public event Action OnNightStart;
    public event Action OnNewDay;

    [SerializeField] private float dayLengthInMinutes = 1f;
    [SerializeField] private float _currentTime;

    private bool _isDaytime = true;
    [SerializeField] private int _daysPassed = 0;

    [SerializeField] private ParticleSystem fogParticleSystem;
    [SerializeField] private bool isFogActive;

    [Header("Ambience Clips")]
    [SerializeField] private AudioClip dayAmbienceClip;
    [SerializeField] private AudioClip nightAmbienceClip;
    [SerializeField] private float ambienceVolume = 0.05f;

    private void Start()
    {
        _currentTime = 4f; // Start at 4 AM
        Debug.Log("DayNightCycle is active");
        UpdateFogState();

        // Play appropriate ambience at start
        if (_isDaytime && dayAmbienceClip != null)
        {
            AudioManager.Instance.PlayAmbience(dayAmbienceClip, ambienceVolume, true);
        }
        else if (!_isDaytime && nightAmbienceClip != null)
        {
            AudioManager.Instance.PlayAmbience(nightAmbienceClip, ambienceVolume, true);
        }
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        _currentTime += Time.deltaTime * (24f / (dayLengthInMinutes * 60f));

        if (_currentTime >= 24f) // A new day
        {
            _currentTime = 0f;
            _daysPassed++;
            OnNewDay?.Invoke();
            Debug.Log("New day started");
            if (!_isDaytime) StartDay();
        }
        else if (_isDaytime && _currentTime >= 18f) // Transition to Night at 6 PM
        {
            Debug.Log("Transition to Night");
            StartNight();
        }
        else if (!_isDaytime && _currentTime >= 6f && _currentTime < 18f) // Transition to Day at 6 AM
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
        UpdateFogState();

        // Play day ambience
        if (dayAmbienceClip != null)
        {
            AudioManager.Instance.PlayAmbience(dayAmbienceClip, ambienceVolume, true);
        }
    }

    private void StartNight()
    {
        _isDaytime = false;
        OnNightStart?.Invoke();
        Debug.Log("Night started.");
        UpdateFogState();

        // Play night ambience
        if (nightAmbienceClip != null)
        {
            AudioManager.Instance.PlayAmbience(nightAmbienceClip, ambienceVolume, true);
        }
    }

    private void UpdateFogState()
    {
        if (fogParticleSystem != null)
        {
            isFogActive = !_isDaytime;
            fogParticleSystem.gameObject.SetActive(isFogActive);
        }
    }

    public float GetCurrentTime() => _currentTime;
    public int GetDaysPassed() => _daysPassed;
    public bool IsDaytime() => _isDaytime;
}

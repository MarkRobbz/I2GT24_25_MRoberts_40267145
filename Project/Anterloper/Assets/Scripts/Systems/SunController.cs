using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private Light sunLight;

    private void Start()
    {
        dayNightCycle = gameObject.GetComponent<DayNightCycle>();
        sunLight = gameObject.GetComponentInChildren<Light>();
        if (sunLight == null)
        {
            Debug.Log("Missing light component for Sun");
        }
    }

    private void Update()
    {
        RotateSun();
    }

    private void RotateSun()
    {
        float currentTime = dayNightCycle.GetCurrentTime();
        float timePercent = currentTime / 24f; // Normalized time [0, 1]
        
        float sunAngle = timePercent * 360f - 90f; //  angle: -90 degrees at midnight, 90 degrees at noon
        
        transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 0f, 0f));
        
        AdjustLightIntensity(sunAngle);
    }

    private void AdjustLightIntensity(float sunAngle)
    {
        
        float intensity = Mathf.Clamp01(Mathf.Sin(sunAngle * Mathf.Deg2Rad)); //smooth intensity transition
        sunLight.intensity = intensity;
    }
}
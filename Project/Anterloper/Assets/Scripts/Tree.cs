using UnityEngine;

public class Tree : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public GameObject logPrefab;
    public int logCount = 3;
    public bool isFallen = false;
    public int regrowTime = 3; // Number of days to regrow
    private int daysSinceFelled = 0;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private DayNightCycle _dayNightCycle;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _dayNightCycle = FindObjectOfType<DayNightCycle>();
        _dayNightCycle.OnNewDay += OnNewDay;
        _rigidbody.isKinematic = true;
    }

    public void ApplyDamage(float damage)
    {
        if (isFallen)
            return;

        health -= damage;
        Debug.Log($"Tree took {damage} damage, health now {health}");

        if (health <= 0)
        {
            FellTree();
        }
    }

    private void FellTree()
    {
        isFallen = true;
        _rigidbody.isKinematic = false; // Let tree fall
        _rigidbody.AddForce(transform.forward * 2f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFallen)
        {
            // spawn logs
            if (collision.gameObject.CompareTag("Ground"))
            {
                SpawnLogs();
                // set tree to inactive
                gameObject.SetActive(false);
            }
        }
    }

    private void SpawnLogs()
    {
        for (int i = 0; i < logCount; i++)
        {
            Instantiate(logPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }

    private void OnNewDay()
    {
        if (isFallen)
        {
            daysSinceFelled++;
            if (daysSinceFelled >= regrowTime)
            {
                Regrow();
            }
        }
    }

    private void Regrow()
    {
        isFallen = false;
        daysSinceFelled = 0;
        health = maxHealth;
        gameObject.SetActive(true);
        _rigidbody.isKinematic = true;
        transform.rotation = Quaternion.identity;
    }

    private void OnDestroy()
    {
        if (_dayNightCycle != null)
        {
            _dayNightCycle.OnNewDay -= OnNewDay;
        }
    }

    
}

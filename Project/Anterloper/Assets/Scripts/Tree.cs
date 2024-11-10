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

    public GameObject stump;     
    public GameObject treeTrunk;  

    private Rigidbody _rigidbody;
    private Collider _collider;
    private DayNightCycle _dayNightCycle;

    private void Start()
    {
        
        _rigidbody = treeTrunk.GetComponent<Rigidbody>();
        _collider = treeTrunk.GetComponent<Collider>();
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
        _rigidbody.isKinematic = false; // Let tree trunk fall
        _rigidbody.AddForce(transform.forward * 2f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFallen)
        {
            // When the tree trunk hits the ground, spawn logs
            if (collision.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Tree has hit the ground. Spawning logs.");
                SpawnLogs();
                treeTrunk.SetActive(false); // Disable tree trunk
            }
        }
    }

    private void SpawnLogs()
    {
        float radius = 1.0f;
        for (int i = 0; i < logCount; i++)
        {
            float angle = i * Mathf.PI * 2 / logCount;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 spawnPosition = stump.transform.position + offset + Vector3.up * 0.5f;

            GameObject log = Instantiate(logPrefab, spawnPosition, Quaternion.identity);

            
            log.transform.Rotate(0, Random.Range(0, 360), 0);

            
            Rigidbody rb = log.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                // Apply a small random force to make logs move naturally
                Vector3 randomForce = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(0.5f, 1.0f),
                    Random.Range(-0.5f, 0.5f)
                );
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
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

        // Reset tree trunk position and rotation
        treeTrunk.transform.position = stump.transform.position + Vector3.up * treeTrunk.GetComponent<Renderer>().bounds.size.y / 2;
        treeTrunk.transform.rotation = Quaternion.identity;

        treeTrunk.SetActive(true);
        _rigidbody.isKinematic = true;
    }
    
    public void OnTrunkCollisionEnter(Collision collision)
    {
        if (isFallen)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Tree has hit the ground. Spawning logs.");
                SpawnLogs();
                treeTrunk.SetActive(false); // Disable tree trunk
            }
        }
    }


    private void OnDestroy()
    {
        if (_dayNightCycle != null)
        {
            _dayNightCycle.OnNewDay -= OnNewDay;
        }
    }
}

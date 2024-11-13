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
    private bool hasSpawnedLogs = false;
    
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

    public void OnTrunkCollisionEnter(Collision collision)
    {
        if (isFallen && !hasSpawnedLogs)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Tree has hit the ground. Spawning logs.");
                
                _collider.enabled = false;

                SpawnLogs();
                hasSpawnedLogs = true;
                
                treeTrunk.SetActive(false);
            }
        }
    }


    private void SpawnLogs()
{
    _collider.enabled = false;
    
    MeshFilter mf = treeTrunk.GetComponent<MeshFilter>();
    if (mf == null)
    {
        Debug.LogError("TreeTrunk has no MeshFilter");
        return;
    }

    Mesh mesh = mf.mesh;
    if (mesh == null)
    {
        Debug.LogError("TreeTrunk's MeshFilter has no mesh");
        return;
    }

    // Get bounds in local space
    Bounds bounds = mesh.bounds;

    // Get local positions of bottom and top
    Vector3 localBottom = bounds.center - new Vector3(0, bounds.extents.y, 0);
    Vector3 localTop = bounds.center + new Vector3(0, bounds.extents.y, 0);

    // Convert to world positions
    Vector3 worldBottom = treeTrunk.transform.TransformPoint(localBottom);
    Vector3 worldTop = treeTrunk.transform.TransformPoint(localTop);

    // Direction along tree trunk
    Vector3 direction = (worldTop - worldBottom).normalized;

    float trunkLength = Vector3.Distance(worldTop, worldBottom);

    int numberOfLogs = logCount; 
    float segmentLength = trunkLength / numberOfLogs;

    for (int i = 0; i < numberOfLogs; i++)
    {
        float distanceAlongTrunk = segmentLength * (i + 0.5f);
        Vector3 spawnPosition = worldBottom + direction * distanceAlongTrunk;
        
        spawnPosition += direction * 0.05f;
        
        GameObject log = Instantiate(logPrefab, spawnPosition, treeTrunk.transform.rotation);
        
        Rigidbody rb = log.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
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
        hasSpawnedLogs = false;

        // Reset tree trunk position/rotation
        treeTrunk.transform.position = stump.transform.position + Vector3.up * (treeTrunk.GetComponent<Renderer>().bounds.size.y / 2);
        treeTrunk.transform.rotation = Quaternion.identity;

        treeTrunk.SetActive(true);
        _rigidbody.isKinematic = true;
        
        _collider.enabled = true;
    }


    
    

 


    private void OnDestroy()
    {
        if (_dayNightCycle != null)
        {
            _dayNightCycle.OnNewDay -= OnNewDay;
        }
    }
}

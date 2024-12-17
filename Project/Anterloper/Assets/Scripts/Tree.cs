using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour, IAttackable
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
    private Vector3 _lastPlayerPosition;

    

    private void Start()
    {
        
        _rigidbody = treeTrunk.GetComponent<Rigidbody>();
        _collider = treeTrunk.GetComponent<Collider>();
        _dayNightCycle = FindObjectOfType<DayNightCycle>();
        _dayNightCycle.OnNewDay += OnNewDay;
        _rigidbody.isKinematic = true;
    }

    public void TakeDamage(float damage)
    {
        if (isFallen)
            return;

        health -= damage;
        Debug.Log($"Tree took {damage} damage, health now {health}");
        
        StartCoroutine(ShakeTree(0.1f, 2f)); // Treeshake plaer feedback

        // Update the player's position when taking damage*
        UpdateLastPlayerPosition();

        if (health <= 0)
        {
            FellTree();
        }
    }
    
    private IEnumerator ShakeTree(float duration, float magnitude)
    {
        Vector3 originalEulerAngles = treeTrunk.transform.localEulerAngles;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float xAngle = originalEulerAngles.x + Random.Range(-1f, 1f) * magnitude;
            float zAngle = originalEulerAngles.z + Random.Range(-1f, 1f) * magnitude;

            treeTrunk.transform.localEulerAngles = new Vector3(xAngle, originalEulerAngles.y, zAngle);
            yield return null;
        }

        // Reset to original rotation
        treeTrunk.transform.localEulerAngles = originalEulerAngles;
    }

    public TargetType GetTargetType()
    {
        return TargetType.Tree;
    }
    private void UpdateLastPlayerPosition()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _lastPlayerPosition = player.transform.position;
        }
        else
        {
            Debug.LogWarning("Player not found; using default position.");
            _lastPlayerPosition = transform.position; // Fallback to tree's position
        }
    }
    private void FellTree()
    {
        isFallen = true;
        _rigidbody.isKinematic = false; // Let tree trunk fall
        
        _rigidbody.constraints = RigidbodyConstraints.None;
        
        _rigidbody.useGravity = true;
        
        Vector3 directionToPlayer = _lastPlayerPosition - treeTrunk.transform.position;
        
        Vector3 fallDirection = -directionToPlayer.normalized;
        
        float treeHeight = treeTrunk.GetComponent<Renderer>().bounds.size.y;
        Vector3 forcePosition = treeTrunk.transform.position + Vector3.up * (treeHeight * 0.8f);
        
        float forceMagnitude = 500f; //Tree mass is 2000 (2 tons)
        Vector3 force = fallDirection * forceMagnitude;
        _rigidbody.AddForceAtPosition(force, forcePosition, ForceMode.Impulse);
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

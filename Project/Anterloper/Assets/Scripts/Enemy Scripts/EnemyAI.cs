using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Stalking, Fleeing, Attacking }
    public EnemyState currentState = EnemyState.Idle;

    public Transform player;
    public float stalkingDistance = 15f;
    public float fleeDistance = 10f;
    public float attackDistance = 5f;

    public float attackDamage = 10f;  //damage per attack
    public float attackCooldown = 1f; //Time between attacks in secs
    private float attackTimer = 0f; //to track cooldown

    private NavMeshAgent _agent;
    private DayNightCycle _dayNightCycle;
    private int _daysPassed;

    private Health _playerHealth;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _dayNightCycle = FindObjectOfType<DayNightCycle>();

        _dayNightCycle.OnNightStart += OnNightStart;
        _dayNightCycle.OnDayStart += OnDayStart;
        _dayNightCycle.OnNewDay += OnNewDay;

        currentState = EnemyState.Idle;
        gameObject.SetActive(false); // Hide enemy during day

        
        _agent.updatePosition = true;
        _agent.updateRotation = false; //Controling rotation manually to move backwards facing player

        if (player != null)
        {
            _playerHealth = player.GetComponent<Health>();
            if (_playerHealth == null)
            {
                Debug.LogError("Player's Health component not found!");
            }
        }
        else
        {
            Debug.LogError("Player Transform not assigned in EnemyAI script!");
        }
    }

    void Update()
    {
        if (currentState == EnemyState.Idle) return;

        attackTimer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Stalking:
                if (distanceToPlayer < fleeDistance)
                {
                    currentState = EnemyState.Fleeing;
                }
                else if (distanceToPlayer < attackDistance)
                {
                    currentState = EnemyState.Attacking;
                }
                else
                {
                    StalkPlayer();
                }
                break;

            case EnemyState.Fleeing:
                if (distanceToPlayer > stalkingDistance)
                {
                    currentState = EnemyState.Stalking;
                }
                else if (distanceToPlayer < attackDistance)
                {
                    currentState = EnemyState.Attacking;
                }
                else
                {
                    FleeFromPlayer();
                }
                break;

            case EnemyState.Attacking:
                if (distanceToPlayer > attackDistance)
                {
                    currentState = EnemyState.Stalking;
                }
                else
                {
                    AttackPlayer();
                }
                break;
        }

        // Face the player in all states except Idle
        if (currentState != EnemyState.Idle)
        {
            FacePlayer();
        }
    }

    void StalkPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - direction * stalkingDistance;
        _agent.SetDestination(targetPosition);
    }

    void FleeFromPlayer()
    {
        // Calculate a point away from the player
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        // Find a valid point on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTarget, out hit, fleeDistance, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
        else
        {
            // If no valid point is found, just move directly away from the player
            _agent.SetDestination(transform.position + fleeDirection * fleeDistance);
        }
    }

    void AttackPlayer()
    {
        // Check if can attack
        if (attackTimer <= 0f)
        {
            if (_playerHealth != null)
            {
                _playerHealth.DecreaseHealth(attackDamage);
                Debug.Log("Enemy attacked the player! Player's current health: " + _playerHealth.CurrentHealth);
            }
            else
            {
                Debug.LogError("Player's Health component is missing!");
            }

            // Reset attack timer
            attackTimer = attackCooldown;
        }

        // Stop moving when attacking
        _agent.SetDestination(transform.position);
    }

    void OnNightStart()
    {
        gameObject.SetActive(true);
        currentState = EnemyState.Stalking;
    }

    void OnDayStart()
    {
        gameObject.SetActive(false);
        currentState = EnemyState.Idle;
    }

    void OnNewDay()
    {
        _daysPassed = _dayNightCycle.GetDaysPassed();
        if (_daysPassed >= 2)
        {
            //*Implement patrolling later*
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        //*Change to head movement later*
        // Add slight randomness (creepy factor)
        float lookAwayChance = .05f; // 5% chance each frame
        if (Random.value < lookAwayChance)
        {
            direction += new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
        }

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}

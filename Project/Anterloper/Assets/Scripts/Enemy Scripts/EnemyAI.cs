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

    public float attackDamage = 10f;       // damage per attack
    public float attackCooldown = 1f;      // Time between attacks in seconds
    private float attackTimer = 0f;        // to track cooldown

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
        gameObject.SetActive(false); // Hide enemy during the day

        
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
    }

    void StalkPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - direction * stalkingDistance;
        _agent.SetDestination(targetPosition);
    }

    void FleeFromPlayer()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        Vector3 fleePosition = transform.position + direction * fleeDistance;
        _agent.SetDestination(fleePosition);
    }

    void AttackPlayer()
    {
        // Face player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

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
            //*Implement patrolling leter*
        }
    }
}

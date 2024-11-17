using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Stalking, Fleeing, Chasing, Attacking }
    public EnemyState currentState = EnemyState.Idle;

    public Transform player;
    public float stalkingDistance = 15f; // Distance to stalk player from
    public float fleeDistance = 10f;     // Distance at which the enemy starts fleeing
    public float chaseDistance = 20f;    // Max distance to chase the player
    public float attackDistance = 5f;    // Distance at which the enemy can attack

    public float attackDamage = 10f;     // Damage per attack
    public float attackCooldown = 1f;    // Time between attacks in seconds
    private float _attackTimer = 0f;     // Tracks attack cooldown

    private NavMeshAgent _agent;
    private DayNightCycle _dayNightCycle;
    private int _daysPassed;

    private Health _playerHealth;

    private bool _canAttack = false;
    private bool _decisionMade = false;   // Tracks if the enemy has made a decision after allowed days
    
    public int attackAfterDays = 2; //  Easy=4, Medium=2, Hard=0

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _dayNightCycle = FindObjectOfType<DayNightCycle>();

        _dayNightCycle.OnNightStart += OnNightStart;
        _dayNightCycle.OnDayStart += OnDayStart;
        _dayNightCycle.OnNewDay += OnNewDay;

        gameObject.SetActive(false); // Hide enemy during day

        _agent.updatePosition = true;
        _agent.updateRotation = false; // Controlling rotation manually to move backwards facing player

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
        
        _daysPassed = _dayNightCycle.GetDaysPassed();
        _canAttack = (_daysPassed >= attackAfterDays);
    }

    void Update()
    {
        // If enemy is inactive, don't process any state
        if (!gameObject.activeSelf) return;

        _attackTimer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Stalking:
                if (_canAttack && !_decisionMade)
                {
                    _decisionMade = true;
                    float randomValue = Random.value;
                    if (randomValue < 0.5f)
                    {
                        // Continue stalking
                        StalkPlayer();
                    }
                    else
                    {
                        // Start chasing the player
                        currentState = EnemyState.Chasing;
                    }
                }
                else
                {
                    // Maintain stalking behavior
                    if (distanceToPlayer < fleeDistance)
                    {
                        currentState = EnemyState.Fleeing;
                    }
                    else if (distanceToPlayer < attackDistance)
                    {
                        if (_canAttack)
                        {
                            // Attack before fleeing
                            AttackPlayer();
                            currentState = EnemyState.Fleeing;
                        }
                        else
                        {
                            currentState = EnemyState.Fleeing;
                        }
                    }
                    else
                    {
                        StalkPlayer();
                    }
                }
                break;

            case EnemyState.Fleeing:
                if (distanceToPlayer > stalkingDistance)
                {
                    currentState = EnemyState.Stalking;
                }
                else
                {
                    FleeFromPlayer();
                }
                break;

            case EnemyState.Chasing:
                if (distanceToPlayer > chaseDistance)
                {
                    // Stop chasing if player is too far
                    currentState = EnemyState.Stalking;
                }
                else if (distanceToPlayer <= attackDistance)
                {
                    currentState = EnemyState.Attacking;
                }
                else
                {
                    ChasePlayer();
                }
                break;

            case EnemyState.Attacking:
                if (distanceToPlayer > attackDistance)
                {
                    // Continue chasing if player moves out of attack range
                    currentState = EnemyState.Chasing;
                }
                else
                {
                    AttackPlayer();
                }
                break;

            case EnemyState.Idle:
                break;
        }

        if (currentState != EnemyState.Idle)
        {
            FacePlayer();
        }
    }

    void StalkPlayer()
    {
        // Follow the player but maintain a certain distance
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - direction * stalkingDistance;
        _agent.SetDestination(targetPosition);
    }

    void FleeFromPlayer()
    {
        // Move away from the player to maintain distance
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

    void ChasePlayer()
    {
        // Chase the player
        _agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        // Stop moving when attacking
        _agent.SetDestination(transform.position);

        // Check if can attack
        if (_attackTimer <= 0f)
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
            _attackTimer = attackCooldown;
        }
    }

    void OnNightStart()
    {
        gameObject.SetActive(true);
        currentState = EnemyState.Stalking;
        _decisionMade = false; // Reset for the night

        // Update _canAttack in case difficulty changed
        _daysPassed = _dayNightCycle.GetDaysPassed();
        _canAttack = (_daysPassed >= attackAfterDays);
    }

    void OnDayStart()
    {
        gameObject.SetActive(false);
        currentState = EnemyState.Idle;
    }

    void OnNewDay()
    {
        _daysPassed = _dayNightCycle.GetDaysPassed();
        if (_daysPassed >= attackAfterDays)
        {
            _canAttack = true;
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

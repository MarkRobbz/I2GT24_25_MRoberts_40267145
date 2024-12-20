using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Stalking, MovingBackwards, Fleeing, Chasing, Attacking }
    public EnemyState currentState = EnemyState.Idle;
    private EnemyState previousState = EnemyState.Idle; // Track previous state

    public Transform player;
    public float stalkingDistance = 15f; 
    public float fleeDistance = 10f;     
    public float chaseDistance = 20f;    
    public float attackDistance = 5f;    

    public float attackDamage = 10f;     
    public float attackCooldown = 1f;    
    private float _attackTimer = 0f;     

    private NavMeshAgent _agent;
    private DayNightCycle _dayNightCycle;
    private int _daysPassed;

    private Health _health;
    private Health _playerHealth;

    private bool _canAttack = false;
    private bool _decisionMade = false;   
    public int attackAfterDays = 2; 

    private EnemyAnimationController _animationController;

    [Header("Monster Audio Clips")]
    [SerializeField] private AudioClip IdleSound;
    [SerializeField] private AudioClip StalkingSound;
    [SerializeField] private AudioClip FleeingSound;
    [SerializeField] private AudioClip ChasingSound;
    [SerializeField] private AudioClip AttackingSound;
    [SerializeField] private AudioClip DeathSound;

    private AudioSource currentLoopingSource;
    private bool isDying = false; // To prevent multiple death triggers

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _dayNightCycle = FindObjectOfType<DayNightCycle>();

        _dayNightCycle.OnNightStart += OnNightStart;
        _dayNightCycle.OnDayStart += OnDayStart;
        _dayNightCycle.OnNewDay += OnNewDay;

        gameObject.SetActive(false); // Hide enemy during day

        _agent.updatePosition = true;
        _agent.updateRotation = false; 

        _health = GetComponent<Health>();
        if (_health == null)
        {
            Debug.LogError("Health component missing on EnemyAI.");
        }

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

        _animationController = GetComponentInChildren<EnemyAnimationController>();
        if (_animationController == null)
        {
            Debug.LogError("EnemyAnimationManager component missing!");
        }

        _daysPassed = _dayNightCycle.GetDaysPassed();
        _canAttack = (_daysPassed >= attackAfterDays);
    }

    void Update()
    {
        // If dying then skips logic
        if (isDying) return;

        if (!gameObject.activeSelf) return;

        if (_health.CurrentHealth <= 0)
        {
            HandleDeath();
            return;
        }

        _attackTimer -= Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Determine next state based on conditions
        EnemyState newState = DetermineState(distanceToPlayer);

        // Check if state changed
        if (newState != currentState)
        {
            OnStateExit(currentState);
            currentState = newState;
            OnStateEnter(currentState);
        }

        // Update logic for current state
        UpdateStateLogic(currentState, distanceToPlayer);

        if (currentState != EnemyState.Idle)
        {
            FacePlayer();
        }

        _animationController.UpdateMovementAnimation();

        previousState = currentState;
    }

    void HandleDeath()
    {
        isDying = true;
        // Stop any movement and sounds
        if (_agent != null && _agent.isActiveAndEnabled) _agent.SetDestination(transform.position);
        StopAmbience(); // Stop any looping sound

        // Play the death sound once
        float clipLength = PlayOneShot(DeathSound, 0.3f);

        // Start a coroutine to destroy after sound finishes
        StartCoroutine(DestroyAfterSound(clipLength));
    }

    private System.Collections.IEnumerator DestroyAfterSound(float length)
    {
        yield return new WaitForSeconds(length);
        Destroy(gameObject);
    }

    EnemyState DetermineState(float distanceToPlayer)
    {
        switch (currentState)
        {
            case EnemyState.Stalking:
                if (_canAttack && !_decisionMade)
                {
                    _decisionMade = true;
                    float randomValue = UnityEngine.Random.value;
                    if (randomValue < 0.5f)
                    {
                        // Continue stalking
                        return EnemyState.Stalking;
                    }
                    else
                    {
                        // Start chasing the player
                        return EnemyState.Chasing;
                    }
                }
                else
                {
                    // Maintain stalking behavior
                    if (distanceToPlayer < fleeDistance)
                    {
                        return EnemyState.Fleeing;
                    }
                    else if (distanceToPlayer < attackDistance)
                    {
                        if (_canAttack)
                        {
                            // Attack before fleeing
                            return EnemyState.Attacking;
                        }
                        else
                        {
                            return EnemyState.MovingBackwards;
                        }
                    }
                    else
                    {
                        return EnemyState.Stalking; 
                    }
                }

            case EnemyState.MovingBackwards:
                if (distanceToPlayer > stalkingDistance)
                {
                    return EnemyState.Stalking;
                }
                else
                {
                    return EnemyState.MovingBackwards;
                }

            case EnemyState.Fleeing:
                if (distanceToPlayer > stalkingDistance)
                {
                    return EnemyState.Stalking;
                }
                else
                {
                    return EnemyState.Fleeing;
                }

            case EnemyState.Chasing:
                if (distanceToPlayer > chaseDistance)
                {
                    // Stop chasing if player is too far
                    return EnemyState.Stalking;
                }
                else if (distanceToPlayer <= attackDistance)
                {
                    return EnemyState.Attacking;
                }
                else
                {
                    return EnemyState.Chasing;
                }

            case EnemyState.Attacking:
                if (distanceToPlayer > attackDistance)
                {
                    return EnemyState.Chasing;
                }
                else
                {
                    return EnemyState.Attacking;
                }

            case EnemyState.Idle:
                return currentState;
        }

        return currentState;
    }

    void UpdateStateLogic(EnemyState state, float distanceToPlayer)
    {
        switch (state)
        {
            case EnemyState.Stalking:
                StalkPlayer(distanceToPlayer);
                break;
            case EnemyState.MovingBackwards:
                MoveBackwards();
                break;
            case EnemyState.Fleeing:
                FleeFromPlayer();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
            case EnemyState.Idle:
                break;
        }
    }

    void OnStateEnter(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                PlayLooping(IdleSound, 0.03f);
                break;
            case EnemyState.Stalking:
                PlayLooping(StalkingSound, 0.03f);
                break;
            case EnemyState.Fleeing:
                PlayLooping(FleeingSound, 0.03f);
                break;
            case EnemyState.Chasing:
                PlayLooping(ChasingSound, 0.03f);
                break;
            case EnemyState.Attacking:
                // Attacking will be handled as a one-shot sound during the attack action
                break;
        }
    }

    void OnStateExit(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
            case EnemyState.Stalking:
            case EnemyState.Fleeing:
            case EnemyState.Chasing:
                StopAmbience();
                break;
            case EnemyState.Attacking:
                break;
        }
    }

    void StalkPlayer(float distanceToPlayer)
    {
        if (!IsAgentReady()) return;
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - direction * stalkingDistance;
        _agent.SetDestination(targetPosition);
    }

    void MoveBackwards()
    {
        if (!IsAgentReady()) return;
        Vector3 direction = (transform.position - player.position).normalized;
        Vector3 targetPosition = transform.position + direction * stalkingDistance;
        _agent.SetDestination(targetPosition);
    }

    void FleeFromPlayer()
    {
        if (!IsAgentReady()) return;
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTarget, out hit, fleeDistance, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
        else
        {
            _agent.SetDestination(transform.position + fleeDirection * fleeDistance);
        }
    }

    void ChasePlayer()
    {
        if (!IsAgentReady()) return;
        _agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        if (!IsAgentReady()) return;
        _agent.SetDestination(transform.position);

        if (_attackTimer <= 0f)
        {
            PlayOneShot(AttackingSound, 0.2f);

            if (_playerHealth != null)
            {
                _playerHealth.DecreaseHealth(attackDamage);
                Debug.Log("Enemy attacked the player! Player's current health: " + _playerHealth.CurrentHealth);
            }
            else
            {
                Debug.LogError("Player's Health component is missing!");
            }

            _attackTimer = attackCooldown;
        }
    }

    void OnNightStart()
    {
        gameObject.SetActive(true);
        currentState = EnemyState.Stalking;
        _decisionMade = false; 
        _daysPassed = _dayNightCycle.GetDaysPassed();
        _canAttack = (_daysPassed >= attackAfterDays);

        OnStateExit(EnemyState.Idle);
        OnStateEnter(EnemyState.Stalking);
    }

    void OnDayStart()
    {
        OnStateExit(currentState);
        currentState = EnemyState.Idle;
        OnStateEnter(currentState);
        gameObject.SetActive(false);
    }

    void OnNewDay()
    {
        _daysPassed = _dayNightCycle.GetDaysPassed();
        if (_daysPassed >= attackAfterDays)
        {
            _canAttack = true;
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float lookAwayChance = .05f;
        if (UnityEngine.Random.value < lookAwayChance)
        {
            direction += new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0, UnityEngine.Random.Range(-0.2f, 0.2f));
        }

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    bool IsAgentReady()
    {
        return (_agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh);
    }

    private void OnDestroy()
    {
        if (_dayNightCycle != null)
        {
            _dayNightCycle.OnNightStart -= OnNightStart;
            _dayNightCycle.OnDayStart -= OnDayStart;
            _dayNightCycle.OnNewDay -= OnNewDay;
        }
    }

    
    float PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            AudioManager.Instance.Play3D(clip, transform, volume);
            return clip.length;
        }
        return 0f;
    }

    void PlayLooping(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            currentLoopingSource = AudioManager.Instance.Play3DLooping(clip, transform, volume);
        }
    }

    void StopAmbience()
    {
        if (currentLoopingSource != null)
        {
            AudioManager.Instance.StopLoopingAudio(currentLoopingSource);
            currentLoopingSource = null;
        }
    }
}

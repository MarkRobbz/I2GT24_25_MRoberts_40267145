using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    private EnemyAI _enemyAI;
    private NavMeshAgent _agent;

    private readonly string SPEED_PARAM = "Speed";
    private readonly string IS_FLEEING_PARAM = "IsFleeing";
    private readonly string HEALTH_PARAM = "Health";
    private readonly string DEATH_TRIGGER = "Death";

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        if (_animator == null)
        {
            Debug.LogError("Animator component missing on EnemyAnimationManager!");
        }

        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent component missing on EnemyAnimationManager!");
        }
    }

    private void Start()
    {
        _enemyAI = GetComponent<EnemyAI>();
        _agent = GetComponent<NavMeshAgent>();

        if (_animator == null) Debug.LogError("Animator component missing on Enemy!");
        if (_enemyAI == null) Debug.LogError("EnemyAI component missing!");
    }

    private void Update()
    {
        if (_animator == null || _enemyAI == null) return;

        // Update movement speed parameter for the blend tree
        float normalizedSpeed = _agent.velocity.magnitude / _agent.speed;
        _animator.SetFloat(SPEED_PARAM, normalizedSpeed);

        // Handle enemy states and parameters
        if (_enemyAI.currentState == EnemyAI.EnemyState.Fleeing)
        {
            _animator.SetBool(IS_FLEEING_PARAM, true);
        }
        else
        {
            _animator.SetBool(IS_FLEEING_PARAM, false);
        }

        // Health check and death trigger
        Health health = GetComponent<Health>();
        if (health != null)
        {
            _animator.SetFloat(HEALTH_PARAM, health.CurrentHealth);
            if (health.CurrentHealth <= 0)
            {
                _animator.SetTrigger(DEATH_TRIGGER);
            }
        }
    }

    public void UpdateMovementAnimation()
    {
        if (_agent != null)
        {
            // Calculate speed as a normalized value (0 to 1)
            float normalizedSpeed = _agent.velocity.magnitude / _agent.speed;
            _animator.SetFloat(SPEED_PARAM, normalizedSpeed);
        }
    }
}
using UnityEngine;

public class Attack : MonoBehaviour
{
    public ToolItem toolItem; // Reference to the equipped tool
    public float attackRange = 3f; // Range to hit
    public LayerMask targetLayer;

    private float attackCooldown = 0f; // Time remaining until next attack
    private float attackRate = 1f;     // Attacks per second

    void Start()
    {
        // Initialise attack rate from a tool item
        if (toolItem != null && toolItem.attackRate > 0)
        {
            attackRate = toolItem.attackRate;
        }
        else
        {
            Debug.LogWarning("ToolItem is null or has invalid attackRate. Using default attack rate.");
        }
    }

    void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    
    public void AttemptAttack()
    {
        if (attackCooldown > 0f)
        {
            // Cannot attack yet
            return;
        }

        PerformAttack();

        // Set cooldown based on tools attack rate
        attackCooldown = 1f / attackRate;
    }

    
    public void PerformAttack()
    {
        if (toolItem == null)
        {
            Debug.LogError("No tool item assigned to the Attack component.");
            return;
        }

        RaycastHit hit;
        Vector3 origin = Camera.main.transform.position + Camera.main.transform.forward * 0.1f;
        Vector3 direction = Camera.main.transform.forward;

        if (Physics.Raycast(origin, direction, out hit, attackRange, targetLayer))
        {
            IAttackable attackable = hit.collider.GetComponentInParent<IAttackable>();
            if (attackable != null)
            {
                TargetType targetType = attackable.GetTargetType();

                // Get the damage value from the ToolItem
                float adjustedDamage = toolItem.GetDamageAgainst(targetType);

                if (adjustedDamage > 0)
                {
                    attackable.TakeDamage(adjustedDamage);
                    Debug.Log($"{toolItem.itemName} attacked {hit.collider.gameObject.name} ({targetType}) for {adjustedDamage} damage.");
                }
                else
                {
                    Debug.Log($"{toolItem.itemName} attacked {hit.collider.gameObject.name} ({targetType}), but it's ineffective.");
                }
            }
            else
            {
                Debug.Log("Hit object does not implement IAttackable.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object.");
        }
    }
}

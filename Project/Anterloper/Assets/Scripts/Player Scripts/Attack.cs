using UnityEngine;

public class Attack : MonoBehaviour
{
    public ToolItem toolItem; // Reference to the equipped tool
    public float attackRange = 3f; // Range to hit
    public LayerMask targetLayer;

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
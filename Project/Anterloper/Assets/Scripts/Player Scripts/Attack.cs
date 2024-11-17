using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage = 10f; 
    public float attackRange = 3f; // Range to hit
    public LayerMask targetLayer; 

    public void PerformAttack()
    {
        RaycastHit hit;
        Vector3 origin = Camera.main.transform.position + Camera.main.transform.forward * 0.1f;
        Vector3 direction = Camera.main.transform.forward;

        
        //Debug.DrawRay(origin, direction * attackRange, Color.red, 1f);

        if (Physics.Raycast(origin, direction, out hit, attackRange, targetLayer))
        {
            IAttackable attackable = hit.collider.GetComponentInParent<IAttackable>();
            if (attackable != null)
            {
                attackable.TakeDamage(damage);
                Debug.Log($"{gameObject.name} attacked {hit.collider.gameObject.name} for {damage} damage.");
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



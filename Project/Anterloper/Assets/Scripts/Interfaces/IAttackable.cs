using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void TakeDamage(float damage);
    TargetType GetTargetType();
}





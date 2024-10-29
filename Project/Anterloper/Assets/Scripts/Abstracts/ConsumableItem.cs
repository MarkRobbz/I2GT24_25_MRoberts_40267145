using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ConsumableItem : BaseItem
{
    public float restoreAmount;

    public abstract void Consume();

    public override void Use(bool isHold)
    {
        if (isHold)
        {
            Consume();
        }
        else
        {
            Debug.Log($"Picked up consumable item: {itemName}.");
        }
    }
}




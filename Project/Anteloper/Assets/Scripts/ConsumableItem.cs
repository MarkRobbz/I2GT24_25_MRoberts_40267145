using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ConsumableItem : BaseItem
{
    public float restoreAmount;

    public abstract void Consume();
}




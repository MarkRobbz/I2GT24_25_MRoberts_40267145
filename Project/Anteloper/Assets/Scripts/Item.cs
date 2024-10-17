using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IUsable
{
    public string itemName;

    public abstract void Use();

    public void DestroyItem()
    {
        Destroy(gameObject);
    }


}

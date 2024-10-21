using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/BaseItem")]
public class BaseItem : ScriptableObject, IUsable
{
    public string itemName; 
    public Sprite itemIcon; 
    public int maxStackSize = 60;

    public void Use()
    {
        
    }
}

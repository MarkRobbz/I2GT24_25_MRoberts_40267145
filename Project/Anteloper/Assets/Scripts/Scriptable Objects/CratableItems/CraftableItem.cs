using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Craftable Item", menuName = "Items/Craftable Item")]
public class CraftableItem : BaseItem
{
    [System.Serializable]
    public class Ingredient
    {
        public BaseItem item;
        public int amount;
    }

    public List<Ingredient> requiredItems = new List<Ingredient>();
    
}
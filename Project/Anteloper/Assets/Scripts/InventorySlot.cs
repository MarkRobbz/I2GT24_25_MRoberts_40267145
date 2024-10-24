using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public BaseItem item;
    public int itemCount;

    public bool IsEmpty()
    {
        return item == null;
    }

    public bool AddItem(BaseItem newItem, int count)
    {
        if (item != null && item == newItem && itemCount < item.maxStackSize)
        {
            int availableSpace = item.maxStackSize - itemCount;
            int amountToAdd = Mathf.Min(count, availableSpace);
            itemCount += amountToAdd;
            return true;
        }
        return false;
    }
}
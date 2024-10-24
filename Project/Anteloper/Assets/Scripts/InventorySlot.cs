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
        if (item == null)
        {
            item = newItem;
            itemCount = count;
            return true;
        }
        else if (item == newItem && itemCount + count <= item.maxStackSize)
        {
            itemCount += count;
            return true;
        }
        return false;
    }
}
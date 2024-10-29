using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public BaseItem equippedItem;
    public GameObject equippedItemModel;
    public Transform itemHolder;

    
    public void EquipItem(BaseItem item)
    {
        equippedItem = item;

        
        if (equippedItemModel != null)
        {
            Destroy(equippedItemModel);
        }

        
        if (item.itemPrefab != null)
        {
            equippedItemModel = Instantiate(item.itemPrefab, itemHolder);
        }
    }

    
    public void UnequipItem()
    {
        equippedItem = null;

        if (equippedItemModel != null)
        {
            Destroy(equippedItemModel);
        }
    }
}
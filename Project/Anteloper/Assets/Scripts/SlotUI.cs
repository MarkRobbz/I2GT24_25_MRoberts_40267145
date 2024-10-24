using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    public Image icon; 
    public TextMeshProUGUI countText;

    public void UpdateSlotUI(InventorySlot slot)
    {
        if (slot.IsEmpty())
        {
            icon.enabled = false;
            countText.text = "";
            Debug.Log("Slot is empty, hiding icon and count text.");
        }
        else
        {
            if (slot.item == null)
            {
                Debug.LogError("Slot item is null!");
                return;
            }
            if (slot.item.itemIcon == null)
            {
                Debug.LogError($"Item '{slot.item.itemName}' does not have an icon assigned!");
                return;
            }

            icon.sprite = slot.item.itemIcon; 
            icon.enabled = true;
            countText.text = slot.itemCount > 1 ? slot.itemCount.ToString() : "";
            Debug.Log($"Updating slot with item: {slot.item.itemName}, Count: {slot.itemCount}");
        }
    }
}
    
    

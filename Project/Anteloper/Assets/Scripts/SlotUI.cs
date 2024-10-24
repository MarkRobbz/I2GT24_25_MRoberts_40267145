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
        }
        else
        {
            icon.sprite = slot.item.itemIcon;
            icon.enabled = true;
            countText.text = slot.itemCount > 1 ? slot.itemCount.ToString() : "";
        }
    }
}
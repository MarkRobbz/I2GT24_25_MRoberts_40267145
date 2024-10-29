using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public Image icon; 
    public TextMeshProUGUI countText;
    
    
    public InventorySlot assignedSlot;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Transform _originalParent;

    private InventoryUI _inventoryUI;
    private Inventory _inventory;
    private PlayerEquipment _playerEquipment;

    private RectTransform _inventoryUIRectTransform;
    private RectTransform _quickAccessUIRectTransform;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _inventory = GameObject.FindObjectOfType<Inventory>();
        _inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        _playerEquipment = GameObject.FindObjectOfType<PlayerEquipment>();
        
        if (_canvasGroup == null)
        { 
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        _canvas = GetComponentInParent<Canvas>();
        
        
        _inventoryUIRectTransform = _inventoryUI.inventoryGrid.GetComponent<RectTransform>();
        if (_inventoryUIRectTransform == null)
        {
            Debug.LogError("Inventory UI RectTransform not found.");
        }

        
        _quickAccessUIRectTransform = _inventoryUI.quickAccessGrid.GetComponent<RectTransform>();
        if (_quickAccessUIRectTransform == null)
        {
            Debug.LogError("Quick Access UI RectTransform not found.");
        }
    }
    
    public void UpdateSlotUI()
    {
        if (assignedSlot == null || assignedSlot.IsEmpty())
        {
            icon.enabled = false;
            icon.sprite = null; // Add this line to reset the sprite
            countText.text = "";
            // Debug.Log("Slot is empty, hiding icon and count text.");
        }
        else
        {
            if (assignedSlot.item == null)
            {
                Debug.LogError("Slot item is null!");
                return;
            }
            if (assignedSlot.item.itemIcon == null)
            {
                Debug.LogError($"Item '{assignedSlot.item.itemName}' does not have an icon assigned!");
                return;
            }

            icon.sprite = assignedSlot.item.itemIcon;
            icon.enabled = true;
            countText.text = assignedSlot.itemCount > 1 ? assignedSlot.itemCount.ToString() : "";
            Debug.Log($"Updating slot with item: {assignedSlot.item.itemName}, Count: {assignedSlot.itemCount}");
        }
    }


    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (assignedSlot != null && !assignedSlot.IsEmpty())
        {
            Debug.Log($"Slot clicked: {assignedSlot.item.itemName}");

           
            if (_inventoryUI != null)
            {
                _inventoryUI.OnInventoryItemClicked(assignedSlot.item);
            }
            else
            {
                Debug.LogError("InventoryUI not found in the scene.");
            }
        }
        else
        {
            Debug.Log("Empty slot clicked.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (assignedSlot == null || assignedSlot.IsEmpty())
        {
            return; // Don't drag empty slots
        }

        _originalPosition = _rectTransform.anchoredPosition;
        _originalParent = transform.parent;
        transform.SetParent(_canvas.transform); // Move to root canvas
        _canvasGroup.blocksRaycasts = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (assignedSlot == null || assignedSlot.IsEmpty())
        {
            return; // Don't drag empty slots
        }

        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (assignedSlot == null || assignedSlot.IsEmpty())
        {
            // Reset position and parent just in case
            transform.SetParent(_originalParent);
            _rectTransform.anchoredPosition = _originalPosition;
            _canvasGroup.blocksRaycasts = true;
            return; // No further action needed
        }

        transform.SetParent(_originalParent);
        _rectTransform.anchoredPosition = _originalPosition;
        _canvasGroup.blocksRaycasts = true;

        // Check if pointer is outside Inventory UI and Quick Access UI
        bool outsideInventoryUI = !RectTransformUtility.RectangleContainsScreenPoint(_inventoryUIRectTransform, Input.mousePosition, _canvas.worldCamera);
        bool outsideQuickAccessUI = !RectTransformUtility.RectangleContainsScreenPoint(_quickAccessUIRectTransform, Input.mousePosition, _canvas.worldCamera);

        if (outsideInventoryUI && outsideQuickAccessUI)
        {
            DropItemIntoWorld();
        }
    }




    public void OnDrop(PointerEventData eventData)
    {
        SlotUI draggedSlot = eventData.pointerDrag.GetComponent<SlotUI>();

        if (draggedSlot != null && draggedSlot != this)
        {
            // Ensure pointer is over this slot
            if (RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, Input.mousePosition, _canvas.worldCamera))
            {
                SwapSlots(draggedSlot);
            }
        }
    }




    private void SwapSlots(SlotUI otherSlot)
    {
        // temp slot to hold data
        InventorySlot tempSlot = new InventorySlot();
        tempSlot.Copy(this.assignedSlot);

        // Swap them
        this.assignedSlot.Copy(otherSlot.assignedSlot);
        otherSlot.assignedSlot.Copy(tempSlot);

        
        this.UpdateSlotUI();
        otherSlot.UpdateSlotUI();
    }
    
    
    private void DropItemIntoWorld()
    {
        if (assignedSlot == null || assignedSlot.IsEmpty())
        {
            Debug.LogWarning("Attempted to drop an empty slot.");
            return;
        }
        if (assignedSlot != null && !assignedSlot.IsEmpty())
        {
            // Check item being dropped is currently equipped
            if (_playerEquipment.equippedItem == assignedSlot.item)
            {
                _playerEquipment.UnequipItem();
                Debug.Log($"Unequipped {assignedSlot.item.itemName} before dropping it into the world.");
            }

            // Instantiate item in world at the player's position
            Vector3 dropPosition = _playerEquipment.transform.position + _playerEquipment.transform.forward * 1.5f; // Distance from player
            Quaternion dropRotation = Quaternion.identity;

            GameObject droppedItem = Instantiate(assignedSlot.item.itemPrefab, dropPosition, dropRotation);

            SetLayerRecursively(droppedItem, LayerMask.NameToLayer("Pickups"));

            ItemPickup itemPickup = droppedItem.GetComponent<ItemPickup>();
            if (itemPickup == null)
            {
                itemPickup = droppedItem.AddComponent<ItemPickup>();
                itemPickup.item = assignedSlot.item;
            }

            _inventory.RemoveItem(assignedSlot.item, 1); 

            UpdateSlotUI();
        }
    }


    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}
    
    

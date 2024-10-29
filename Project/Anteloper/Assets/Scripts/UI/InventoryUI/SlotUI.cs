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

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        
        if (_canvasGroup == null)
        { 
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        _canvas = GetComponentInParent<Canvas>();
        
        _inventoryUI = FindObjectOfType<InventoryUI>();
        if (_inventoryUI == null)
        {
            Debug.LogError("InventoryUI not found in the scene.");
        }
    }
    public void UpdateSlotUI()
    {
        if (assignedSlot == null || assignedSlot.IsEmpty())
        {
            icon.enabled = false;
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
        if (icon.sprite == null)
        {
            return; // Dont drag empty slots
        }

        _originalPosition = _rectTransform.anchoredPosition;
        _originalParent = transform.parent;
        transform.SetParent(_canvas.transform); // Move root canvas to top
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (icon.sprite == null)
        {
            return;
        }

        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_originalParent);
        _rectTransform.anchoredPosition = _originalPosition;
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        SlotUI draggedSlot = eventData.pointerDrag.GetComponent<SlotUI>();

        if (draggedSlot != null && draggedSlot != this)
        {
            SwapSlots(draggedSlot);
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
    
    

}
    
    

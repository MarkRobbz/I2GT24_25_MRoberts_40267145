using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject slotPrefab;
    public Transform inventoryGrid;
    public Transform quickAccessGrid;

    private Inventory _inventory;
    private CraftingUI _craftingUI;

    private List<SlotUI> _inventorySlotUIs = new List<SlotUI>();
    private List<SlotUI> _quickAccessSlotUIs = new List<SlotUI>();
    
    private PlayerEquipment _playerEquipment;

    void Start()
    {
        bool wasActive = inventoryUI.activeSelf;

        // Temp activate for initialization
        if (!wasActive)
        {
            inventoryUI.SetActive(true);
        }

        _craftingUI = FindObjectOfType<CraftingUI>();
        _inventory = FindObjectOfType<Inventory>();
        _playerEquipment = FindObjectOfType<PlayerEquipment>();
        _inventory.OnInventoryChanged += UpdateUI;

        InitialiseInventoryUI();
        InitialiseQuickAccessUI();

        // Set to inactive 
        if (!wasActive)
        {
            inventoryUI.SetActive(false);
        }

        UpdateCursorState();
    }


    
    private void InitialiseInventoryUI()
    {
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var slot in _inventory.inventorySlots)
        {
            GameObject newSlotGO = Instantiate(slotPrefab, inventoryGrid);
            SlotUI slotUI = newSlotGO.GetComponent<SlotUI>();
            slotUI.assignedSlot = slot; // Assign the InventorySlot
            slotUI.SetInventoryUI(this);// Set InventoryUI reference
            slotUI.SetPlayerEquipment(_playerEquipment);
            _inventorySlotUIs.Add(slotUI);
        }

        UpdateInventoryUI();
    }

    private void InitialiseQuickAccessUI()
    {
        foreach (Transform child in quickAccessGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var slot in _inventory.quickAccessSlots)
        {
            GameObject newSlotGO = Instantiate(slotPrefab, quickAccessGrid);
            SlotUI slotUI = newSlotGO.GetComponent<SlotUI>();
            slotUI.assignedSlot = slot; // Assign the InventorySlot
            slotUI.SetInventoryUI(this); // Set InventoryUI reference
            slotUI.SetPlayerEquipment(_playerEquipment);
            _quickAccessSlotUIs.Add(slotUI);
        }

        UpdateQuickAccessUI();
    }

    
    public void OnInventoryItemClicked(BaseItem item)
    {
        if (_playerEquipment.equippedItem == item)
        {
            _playerEquipment.UnequipItem(); // already equipped so unequip
            Debug.Log($"Unequipped {item.itemName}");
        }
        else
        {
            // Equip the item
            if (item.itemPrefab != null)
            {
                _playerEquipment.EquipItem(item);
                Debug.Log($"Equipped {item.itemName}");
            }
            else
            {
                Debug.Log($"{item.itemName} cannot be equipped; itemPrefab is null.");
            }
        }
    }


    void UpdateUI()
    {
        UpdateInventoryUI();
        UpdateQuickAccessUI();
    }

    void UpdateInventoryUI()
    {
        foreach (var slotUI in _inventorySlotUIs)
        {
            slotUI.UpdateSlotUI();
        }
    }

    void UpdateQuickAccessUI()
    {
        foreach (var slotUI in _quickAccessSlotUIs)
        {
            slotUI.UpdateSlotUI();
        }
    }

    public bool IsInventoryUIActive()
    {
        return inventoryUI.activeSelf;
    }
   
    public void ToggleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    
        if (inventoryUI.activeSelf && _craftingUI.IsCraftingUIActive())
        {
            _craftingUI.ToggleCraftingUI(); // Close Crafting UI if it's open
        }

        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        if (inventoryUI.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    
    void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.OnInventoryChanged -= UpdateUI;
        }
    }
}
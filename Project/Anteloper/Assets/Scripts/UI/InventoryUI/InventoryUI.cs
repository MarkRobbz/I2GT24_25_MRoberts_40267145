using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Start()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        _craftingUI = FindObjectOfType<CraftingUI>();
        _inventory = FindObjectOfType<Inventory>();
        _inventory.OnInventoryChanged += UpdateUI;
        
        InitialiseInventoryUI();
        InitialiseQuickAccessUI();
       
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
            slotUI.assignedSlot = slot; // Assign InventorySlot
            _quickAccessSlotUIs.Add(slotUI);
        }

        UpdateQuickAccessUI();
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
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

    private List<SlotUI> inventorySlotUIs = new List<SlotUI>();
    private List<SlotUI> quickAccessSlotUIs = new List<SlotUI>();

    void Start()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        _inventory = FindObjectOfType<Inventory>();
        _inventory.onInventoryChanged += UpdateUI;

        InitialiseInventoryUI();
        InitialiseQuickAccessUI();
    }

    private void InitialiseInventoryUI()
    {
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (InventorySlot slot in _inventory.inventorySlots)
        {
            GameObject newSlotGO = Instantiate(slotPrefab, inventoryGrid);
            SlotUI slotUI = newSlotGO.GetComponent<SlotUI>();
            inventorySlotUIs.Add(slotUI);
        }

        UpdateInventoryUI();
    }

    private void InitialiseQuickAccessUI()
    {
        foreach (Transform child in quickAccessGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (InventorySlot slot in _inventory.quickAccessSlots)
        {
            GameObject newSlotGO = Instantiate(slotPrefab, quickAccessGrid);
            SlotUI slotUI = newSlotGO.GetComponent<SlotUI>();
            quickAccessSlotUIs.Add(slotUI);
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
        for (int i = 0; i < _inventory.inventorySlots.Count; i++)
        {
            inventorySlotUIs[i].UpdateSlotUI(_inventory.inventorySlots[i]);
        }
    }

    void UpdateQuickAccessUI()
    {
        for (int i = 0; i < _inventory.quickAccessSlots.Count; i++)
        {
            quickAccessSlotUIs[i].UpdateSlotUI(_inventory.quickAccessSlots[i]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.onInventoryChanged -= UpdateUI;
        }
    }
}
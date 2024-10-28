using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public GameObject craftingUIObj;
    public Transform recipeListContent; 
    public GameObject recipeSlotPrefab; 
    
    public List<CraftableItem> craftableItems; 

    private Inventory _inventory;
    private InventoryUI _inventoryUI;

    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
        _inventoryUI = FindObjectOfType<InventoryUI>();
        PopulateCraftableItems();
        ToggleCraftingUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCraftingUI();
        }
    }

    void PopulateCraftableItems()
    {
        foreach (Transform child in recipeListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (CraftableItem craftableItem in craftableItems)
        {
            GameObject recipeSlotGO = Instantiate(recipeSlotPrefab, recipeListContent);
            RecipeSlotUI recipeSlotUI = recipeSlotGO.GetComponent<RecipeSlotUI>();
            recipeSlotUI.Initialise(craftableItem, _inventory);
        }
    }

    public bool IsCraftingUIActive()
    {
        return craftingUIObj.activeSelf;
    }

    public void ToggleCraftingUI()
    {
        craftingUIObj.SetActive(!craftingUIObj.activeSelf);
    
        if (craftingUIObj.activeSelf && _inventoryUI.IsInventoryUIActive())
        {
            _inventoryUI.ToggleInventoryUI(); // Close Inventory UI if it's open
        }

        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        if (craftingUIObj.activeSelf)
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

}
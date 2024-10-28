using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public GameObject craftingCanvas;
    public Transform recipeListContent; // Content GameObject under ScrollRect
    public GameObject recipeSlotPrefab; // Prefab for each craftable item

    public List<CraftableItem> craftableItems; // Assign in the Inspector

    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>(); // Reference to the player's inventory
        PopulateCraftableItems();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            craftingCanvas.SetActive(!craftingCanvas.activeSelf);
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
            recipeSlotUI.Initialise(craftableItem, inventory);
        }
    }
}
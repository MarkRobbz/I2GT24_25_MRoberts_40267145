using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public GameObject craftingCanvas;
    public Transform recipeListContent; 
    public GameObject recipeSlotPrefab; 
    
    public List<CraftableItem> craftableItems; 

    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>(); 
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
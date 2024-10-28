using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSlotUI : MonoBehaviour
{
    public Image resultIcon;
    public TextMeshProUGUI resultNameText;
    public TextMeshProUGUI ingredientsText;
    public Button craftButton;

    private CraftableItem craftableItem;
    private Inventory inventory;

    public void Initialise(CraftableItem craftableItem, Inventory inventory)
    {
        this.craftableItem = craftableItem;
        this.inventory = inventory;
        inventory.OnInventoryChanged += UpdateIngredientsText; 

        resultIcon.sprite = craftableItem.itemIcon;
        resultNameText.text = craftableItem.itemName;

        UpdateIngredientsText();
        UpdateCraftButton();

        craftButton.onClick.AddListener(OnCraftButtonClicked);
    }

    void UpdateIngredientsText()
    {
        ingredientsText.text = "";
        foreach (var ingredient in craftableItem.requiredItems)
        {
            int totalInInventory = inventory.GetItemCount(ingredient.item);
            string color = totalInInventory >= ingredient.amount ? "green" : "red";
            ingredientsText.text += $"{ingredient.item.itemName} x{ingredient.amount} (<color={color}>{totalInInventory}</color>)\n";
        }
    }

    void UpdateCraftButton()
    {
        craftButton.interactable = CanCraft();
    }

    bool CanCraft()
    {
        foreach (var ingredient in craftableItem.requiredItems)
        {
            int totalInInventory = inventory.GetItemCount(ingredient.item);
            if (totalInInventory < ingredient.amount)
            {
                return false;
            }
        }
        return true;
    }

    void OnCraftButtonClicked()
    {
        if (CanCraft())
        {
            CraftItem();
        }
    }

    void CraftItem()
    {
        
        foreach (var ingredient in craftableItem.requiredItems) // Deduct materials
        {
            inventory.RemoveItem(ingredient.item, ingredient.amount);
        }
        
        inventory.AddItem(craftableItem, 1);
        
        UpdateCraftButton();
        UpdateIngredientsText();
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateIngredientsText;
        }
    }
}

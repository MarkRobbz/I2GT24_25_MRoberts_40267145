using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSlotUI : MonoBehaviour
{
    public Image resultIcon;
    public TextMeshProUGUI resultNameText;
    public TextMeshProUGUI ingredientsText;
    public Button craftButton;

    private CraftableItem _craftableItem;
    private Inventory _inventory;

    public void Initialise(CraftableItem craftableItem, Inventory inventory)
    {
        this._craftableItem = craftableItem;
        this._inventory = inventory;
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
        foreach (var ingredient in _craftableItem.requiredItems)
        {
            int totalInInventory = _inventory.GetItemCount(ingredient.item);
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
        foreach (var ingredient in _craftableItem.requiredItems)
        {
            int totalInInventory = _inventory.GetItemCount(ingredient.item);
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
        
        foreach (var ingredient in _craftableItem.requiredItems) // Deduct materials
        {
            _inventory.RemoveItem(ingredient.item, ingredient.amount);
        }
        
        _inventory.AddItem(_craftableItem, 1);
        
        UpdateCraftButton();
        UpdateIngredientsText();
    }

    private void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.OnInventoryChanged -= UpdateIngredientsText;
        }
    }
}

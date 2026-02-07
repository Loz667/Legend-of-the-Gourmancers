using LotG.Inventories;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Scriptable Objects/Crafting/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string RecipeName;
    public Sprite RecipeIcon;
    public List<InventoryItem> requiredIngredients;
    public InventoryItem Dish;

    public bool CanCraft(Inventory inventory)
    {
        foreach (InventoryItem ingredient in requiredIngredients)
        {
            bool foundIngredient = inventory.HasItem(ingredient);
            if (!foundIngredient)
            {
                return false;
            }
        }
        return true;
    }

    public void Craft(Inventory inventory)
    {
        if (CanCraft(inventory))
        {
            foreach (InventoryItem ingredient in requiredIngredients)
            {
                inventory.RemoveItem(ingredient);
            }
        }        
        
        inventory.AddToFirstEmptySlot(Dish, 1);        
    }
}

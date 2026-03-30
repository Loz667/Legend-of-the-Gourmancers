using System.Collections.Generic;
using UnityEngine;
using LotG.Inventories;
using LotG.Events;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Scriptable Objects/Crafting/Create Recipe")]
public class RecipeSO : ScriptableObject
{
    public string RecipeName;
    public Sprite RecipeIcon;
    public List<ItemSO> requiredIngredients;
    public ItemSO Dish;

    public bool CanCraft(InventorySO inventory)
    {
        foreach (ItemSO ingredient in requiredIngredients)
        {
            bool foundIngredient = inventory.HasItem(ingredient);
            if (!foundIngredient)
            {
                return false;
            }
        }
        return true;
    }

    public void Craft(InventorySO inventory)
    {
        if (CanCraft(inventory))
        {
            foreach (ItemSO ingredient in requiredIngredients)
            {
                inventory.RemoveItem(ingredient);
            }
        }

        GameEventsManager.instance.miscEvents.RecipeCrafted();
        inventory.AddItemToFirstEmptySlot(Dish, 1);        
    }
}

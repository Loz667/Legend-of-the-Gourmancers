using LotG.Inventories;
using UnityEngine;
using UnityEngine.UI;

public class RecipeButtonUI : MonoBehaviour
{
    [SerializeField] Inventory playerInventory;

    [SerializeField] RecipeSO recipe = null;
    Image recipeIcon;

    private void Awake()
    {
        recipeIcon = GetComponent<Image>();
        recipeIcon.sprite = recipe.RecipeIcon;
    }

    private void Update()
    {
        if (!recipe.CanCraft(playerInventory))
        {
            transform.GetComponent<Button>().interactable = false;
        }
        else
        {
            transform.GetComponent<Button>().interactable = true;
        }
    }

    public void OnButtonClick()
    {
        if (recipe != null)
        {
            if (recipe.CanCraft(playerInventory))
            {
                if (playerInventory.HasSpaceFor(recipe.Dish))
                {
                    recipe.Craft(playerInventory);
                }
                else
                {
                    Debug.Log("Inventory is full");
                }
            }
            else
            {
                Debug.Log("Not enough ingredients");
            }
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeButtonSingleUI : MonoBehaviour
{
    [SerializeField] private Image recipeImage;
    [SerializeField] private TextMeshProUGUI recipeName;

    public void SetRecipe(RecipeSO recipe)
    {
        recipeImage.sprite = recipe.recipeIcon;
        recipeName.text = recipe.recipeName;
    }
}

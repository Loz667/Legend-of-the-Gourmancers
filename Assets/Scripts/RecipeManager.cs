using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    //[SerializeField] private RecipeListSO availableRecipes;
    
    [SerializeField]private List<RecipeSO> recipeList;
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private Transform recipeButtonTemplate;

    private void Awake()
    {
        //recipeButtonTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        PopulateRecipes();
    }

    private void PopulateRecipes()
    {
        foreach (Transform child in recipeContainer)
        {
            if (child == recipeButtonTemplate) continue;
            Destroy(child.gameObject);
        }
        
        foreach (RecipeSO recipe in recipeList)
        {
            Transform button = Instantiate(recipeButtonTemplate, recipeContainer);
            recipeButtonTemplate.gameObject.SetActive(true);
            button.GetComponent<RecipeButtonSingleUI>().SetRecipe(recipe);
            //button.GetComponent<Button>().onClick.AddListener(() => cookingManager.StartCooking(recipe));
        }
    }
}

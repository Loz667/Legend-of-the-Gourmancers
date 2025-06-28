using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO ingredient;
    }
    
    [SerializeField] private List<KitchenObjectSO> allowedIngredients;
    private List<KitchenObjectSO> _ingredientList = new List<KitchenObjectSO>();

    public bool TryAddIngredient(KitchenObjectSO ingredient)
    {
        if (!allowedIngredients.Contains(ingredient))
        {
            return false;
        }
        
        if (_ingredientList.Contains(ingredient))
        {
            //Ingredient not required
            return false;
        }
        else
        {
            _ingredientList.Add(ingredient);
            
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs 
                { ingredient = ingredient });
            return true;
        }
    }
}

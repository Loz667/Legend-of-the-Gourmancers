using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalised;
    }

    public event EventHandler OnCut;
    
    [SerializeField] private CuttingRecipeSO[] recipeArray;
    
    private int _cuttingProgress;
    
    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            //There is no KitchenObject on this counter
            //so check if player is carrying something
            if (player.HasKitchenObject())
            {
                //The player is carrying a valid KitchenObject
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Sets it down on the counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    _cuttingProgress = 0;
                    
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                    
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        progressNormalised = (float)_cuttingProgress / cuttingRecipeSO.maxRequiredCuts
                    });
                }
            }
            else
            {
                //Player is not carrying anything
            }
        }
        else
        {
            //There is a KitchenObject on this counter
            //so check if player is carrying something
            if (player.HasKitchenObject())
            {
                //The player is carrying a KitchenObject
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(PlayerController player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            //Only cut if there is valid KitchenObject
            _cuttingProgress++;
            
            OnCut?.Invoke(this, EventArgs.Empty);
            
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
            
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                progressNormalised = (float)_cuttingProgress / cuttingRecipeSO.maxRequiredCuts
            });
            
            if (_cuttingProgress >= cuttingRecipeSO.maxRequiredCuts)
            {
                KitchenObjectSO output = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(output, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeWithInput(input);
        return cuttingRecipeSO != null;
    }
    
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeWithInput(input);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeWithInput(KitchenObjectSO input)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in recipeArray)
        {
            if (cuttingRecipeSO.input == input)
            {
                return cuttingRecipeSO;
            }
        }

        return null;
    }
}

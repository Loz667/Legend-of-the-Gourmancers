using System;
using UnityEngine;

public class CookingFireCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    public enum State
    {
        Idle,
        Cooking,
        Cooked,
        Burnt
    }

    [SerializeField] private CookingRecipeSO[] cookingRecipes;
    [SerializeField] private BurntRecipeSO[] burntRecipes;

    private CookingRecipeSO _recipe;
    private BurntRecipeSO _burntRecipe;
    private State _currentState = State.Idle;
    private float _cookingTimer;
    private float _timeToBurnt;

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_currentState)
            {
                case State.Idle:
                    break;
                case State.Cooking:
                    _cookingTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalised = _cookingTimer / _recipe.maxCookingTime
                    });

                    if (_cookingTimer > _recipe.maxCookingTime)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_recipe.output, this);
                        
                        _currentState = State.Cooked;
                        _timeToBurnt = 0f;
                        _burntRecipe = GetBurntRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = _currentState });
                    }
                    break;
                case State.Cooked:
                    _timeToBurnt += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalised = _timeToBurnt / _burntRecipe.timeToBurnt
                    });

                    if (_timeToBurnt > _burntRecipe.timeToBurnt)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_burntRecipe.output, this);
                        
                        _currentState = State.Burnt;
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = _currentState });
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalised = 0f
                        });
                    }
                    break;
                case State.Burnt:
                    break;
            }
        }
    }

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
                    //Begins cooking process
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    _recipe = GetCookingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());

                    _currentState = State.Cooking;
                    _cookingTimer = 0f;
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = _currentState });
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalised = _cookingTimer / _recipe.maxCookingTime
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
                //Check if player has a plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
                {
                    //Player is holding a plate so add ingredient to plate
                    if (plate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        
                        _currentState = State.Idle;
                
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = _currentState });
                
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalised = 0f
                        });
                    }
                }
            }
            else
            {
                //The player is carrying nothing
                GetKitchenObject().SetKitchenObjectParent(player);

                _currentState = State.Idle;
                
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = _currentState });
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalised = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        CookingRecipeSO cookingRecipe = GetCookingRecipeWithInput(input);
        return cookingRecipe != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        CookingRecipeSO cookingRecipe = GetCookingRecipeWithInput(input);
        if (cookingRecipe != null)
        {
            return cookingRecipe.output;
        }
        else
        {
            return null;
        }
    }

    private CookingRecipeSO GetCookingRecipeWithInput(KitchenObjectSO input)
    {
        foreach (CookingRecipeSO cookingRecipe in cookingRecipes)
        {
            if (cookingRecipe.input == input)
            {
                return cookingRecipe;
            }
        }

        return null;
    }
    
    private BurntRecipeSO GetBurntRecipeWithInput(KitchenObjectSO input)
    {
        foreach (BurntRecipeSO burntRecipe in burntRecipes)
        {
            if (burntRecipe.input == input)
            {
                return burntRecipe;
            }
        }

        return null;
    }
}
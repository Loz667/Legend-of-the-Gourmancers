using LotG.Events;
using LotG.Input;
using LotG.Inventories;
using UnityEngine;
using UnityEngine.UI;

public class RecipeButtonUI : MonoBehaviour
{
    [SerializeField] InventorySO playerInventory;
    [SerializeField] RecipeSO recipe = null;
    [SerializeField] AudioSource fxPlayer;
    [SerializeField] AudioClip craftClip;
    [SerializeField] ParticleSystem craftParticles;

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

    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.OnSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.OnSubmitPressed -= SubmitPressed;
    }

    private void SubmitPressed(InputEventContext inputEventContext)
    {
        if (!inputEventContext.Equals(InputEventContext.DEFAULT)) return;

        CraftRecipe();
    }

    public void CraftRecipe()
    {
        if (recipe != null)
        {
            if (recipe.CanCraft(playerInventory))
            {
                if (playerInventory.HasSpaceFor(recipe.Dish))
                {
                    recipe.Craft(playerInventory);
                    fxPlayer.PlayOneShot(craftClip);
                    craftParticles.Play();
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

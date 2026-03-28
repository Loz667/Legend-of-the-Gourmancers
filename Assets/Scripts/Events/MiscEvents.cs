using System;

public class MiscEvents
{
    public event Action OnIngredientsCollected;
    public void IngredientsCollected()
    {
        if (OnIngredientsCollected != null)
        {
            OnIngredientsCollected();
        }
    }

    public event Action OnRecipeCrafted;
    public void RecipeCrafted()
    {
        if (OnRecipeCrafted != null)
        {
            OnRecipeCrafted();
        }
    }

    public event Action OnBattleCompleted;
    public void BattleCompleted()
    {
        if (OnBattleCompleted != null)
        {
            OnBattleCompleted();
        }
    }
}

using System;

namespace LotG.Events
{
    public class MiscEvents
    {
        public event Action onDisablePlayerMovement;
        public void DisablePlayerMovement()
        {
            if (onDisablePlayerMovement != null)
            {
                onDisablePlayerMovement();
            }
        }

        public event Action onEnablePlayerMovement;
        public void EnablePlayerMovement()
        {
            if (onEnablePlayerMovement != null)
            {
                onEnablePlayerMovement();
            }
        }

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
}

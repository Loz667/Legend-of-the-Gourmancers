using LotG.Events;

namespace LotG.QuestSystem
{
    public class CraftRecipeQuestStep : QuestStep
    {
        private int recipesCrafted = 0;
        private int recipesToCraft = 1;

        private void OnEnable()
        {
            GameEventsManager.instance.miscEvents.OnRecipeCrafted += HandleRecipeCrafted;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.miscEvents.OnRecipeCrafted -= HandleRecipeCrafted;
        }

        private void HandleRecipeCrafted()
        {
            if (recipesCrafted < recipesToCraft)
            {
                recipesCrafted++;
                UpdateState();
            }

            if (recipesCrafted >= recipesToCraft)
            {
                CompletedQuestStep();
            }
        }

        private void UpdateState()
        {
            string state = recipesCrafted.ToString();
            ChangeState(state);
        }

        protected override void SetQuestStepState(string state)
        {
            this.recipesCrafted = System.Int32.Parse(state);
            UpdateState();
        }
    }
}

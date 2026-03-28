namespace LotG.QuestSystem
{
    public class CollectIngredientsQuestStep : QuestStep
    {
        private int ingredientsCollected = 0;
        private int ingredientsToCollect = 1;

        private void OnEnable()
        {
            GameEventsManager.instance.miscEvents.OnIngredientsCollected += HandleIngredientsCollected;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.miscEvents.OnIngredientsCollected -= HandleIngredientsCollected;
        }

        private void HandleIngredientsCollected()
        {
            if (ingredientsCollected < ingredientsToCollect)
            {
                ingredientsCollected++;
                UpdateState();
            }

            if (ingredientsCollected >= ingredientsToCollect)
            {
                CompletedQuestStep();
            }
        }

        private void UpdateState()
        {
            string state = ingredientsCollected.ToString();
            ChangeState(state);
        }

        protected override void SetQuestStepState(string state)
        {
            this.ingredientsCollected = System.Int32.Parse(state);
            UpdateState();
        }
    }
}

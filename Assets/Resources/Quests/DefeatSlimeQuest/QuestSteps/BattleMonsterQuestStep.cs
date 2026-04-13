using LotG.Events;

namespace LotG.QuestSystem
{
    public class BattleMonsterQuestStep : QuestStep
    {
        private int battlesWon = 0;
        private int battlesToWin = 1;

        private void OnEnable()
        {
            GameEventsManager.instance.miscEvents.OnBattleCompleted += HandleBattleWon;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.miscEvents.OnBattleCompleted -= HandleBattleWon;
        }

        private void Start()
        {
            UpdateState();
        }

        private void HandleBattleWon()
        {
            if (battlesWon < battlesToWin)
            {
                battlesWon++;
                UpdateState();
            }

            if (battlesWon >= battlesToWin)
            {
                CompletedQuestStep();
            }
        }

        private void UpdateState()
        {
            string state = battlesWon.ToString();
            string status = "Defeat Slime";
            ChangeState(state, status);
        }

        protected override void SetQuestStepState(string state)
        {
            this.battlesWon = System.Int32.Parse(state);
            UpdateState();
        }
    }
}
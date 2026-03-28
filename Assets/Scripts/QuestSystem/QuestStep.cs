using UnityEngine;

namespace LotG.QuestSystem
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool IsCompleted = false;

        private string questId;
        private int questStepIndex;

        public void InitialiseQuestStep(string questId, int questStpeIndex, string questStepState)
        {
            this.questId = questId;
            this.questStepIndex = questStpeIndex;
            if (questStepState != null && questStepState != "")
            {
                SetQuestStepState(questStepState);
            }
        }

        protected void CompletedQuestStep()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                GameEventsManager.instance.questEvents.AdvanceQuest(questId);
                Destroy(this.gameObject);
            }
        }

        protected void ChangeState(string newState)
        {
            GameEventsManager.instance.questEvents.QuestStepStateChange(questId, questStepIndex, new QuestStepState(newState));
        }

        protected abstract void SetQuestStepState(string state);
    }
}

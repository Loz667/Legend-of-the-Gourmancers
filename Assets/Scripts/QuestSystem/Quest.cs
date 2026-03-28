using UnityEngine;

namespace LotG.QuestSystem
{
    public class Quest
    {
        public QuestInfoSO questInfo;
        public QuestState questState;
        private QuestStepState[] questStepStates;

        private int currentQuestStepIndex;

        public Quest(QuestInfoSO questInfo)
        {
            this.questInfo = questInfo;
            this.questState = QuestState.REQUIREMENTS_NOT_MET;
            this.questStepStates = new QuestStepState[questInfo.questStepPrefabs.Length];
            for (int i = 0; i < questStepStates.Length; i++)
            {
                questStepStates[i] = new QuestStepState();
            }
            this.currentQuestStepIndex = 0;
        }

        public Quest(QuestInfoSO questInfo, QuestState questState, int currentQuestStepIndex, QuestStepState[] questStepStates)
        {
            this.questInfo = questInfo;
            this.questState = questState;
            this.currentQuestStepIndex = currentQuestStepIndex;
            this.questStepStates = questStepStates;

            if (this.questStepStates.Length != this.questInfo.questStepPrefabs.Length)
            {
                Debug.LogWarning("Quest step Prefabs and Quest step states are "
                    + "of different lengths. This indicates that something changed "
                    + "in the Quest Info and the saved data is now out of sync. "
                    + "Reset your data - as this might cause issues. Quest ID: " + this.questInfo.QuestId);
            }
        }

        public QuestData GetQuestData()
        {
            return new QuestData(questState, currentQuestStepIndex, questStepStates);
        }

        public void MoveToNextQuestStep()
        {
            currentQuestStepIndex++;
        }

        public bool CurrentQuestStepExists()
        {
            return currentQuestStepIndex < questInfo.questStepPrefabs.Length;
        }

        public void InstantiateCurrentQuestStep(Transform parent)
        {
            GameObject currentQuestStep = GetCurrentQuestStep();
            if (currentQuestStep != null)
            {
                QuestStep questStep = Object.Instantiate<GameObject>(currentQuestStep, parent).GetComponent<QuestStep>();
                questStep.InitialiseQuestStep(questInfo.QuestId, currentQuestStepIndex, questStepStates[currentQuestStepIndex].stepState);
            }
        }

        public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
        {
            if (stepIndex < questStepStates.Length)
            {
                questStepStates[stepIndex].stepState = questStepState.stepState;
            }
            else
            {
                Debug.LogWarning($"Attempting to store quest step state for step index {stepIndex} which is out of bounds for quest: {questInfo.questName}");
            }
        }

        private GameObject GetCurrentQuestStep()
        {
            GameObject currentQuestStep = null;
            if (CurrentQuestStepExists())
            {
                currentQuestStep = questInfo.questStepPrefabs[currentQuestStepIndex];
            }
            else
            {
                Debug.LogWarning($"No more quest steps available for quest: {questInfo.questName}");
            }
            return currentQuestStep;
        }
    }
}

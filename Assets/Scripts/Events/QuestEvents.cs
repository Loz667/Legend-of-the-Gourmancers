using System;
using UnityEngine;

namespace LotG.QuestSystem
{
    public class QuestEvents
    {
        public event Action<string> OnQuestStarted;
        public void StartQuest(string questId)
        {
            if (OnQuestStarted != null)
            {
                OnQuestStarted(questId);
            }
        }

        public event Action<string> OnAdvanceQuest;
        public void AdvanceQuest(string questId)
        {
            if (OnAdvanceQuest != null)
            {
                OnAdvanceQuest(questId);
            }
        }

        public event Action<string> OnQuestCompleted;
        public void CompleteQuest(string questId)
        {
            if (OnQuestCompleted != null)
            {
                OnQuestCompleted(questId);
            }
        }

        public event Action<Quest> OnQuestStateChange;
        public void QuestStateChange(Quest quest)
        {
            if (OnQuestStateChange != null)
            {
                OnQuestStateChange(quest);
            }
        }

        public event Action<string, int, QuestStepState> OnQuestStepStateChange;
        public void QuestStepStateChange(string questId, int questStepIndex, QuestStepState questStepState)
        {
            if (OnQuestStepStateChange != null)
            {
                OnQuestStepStateChange(questId, questStepIndex, questStepState);
            }
        }
    }
}

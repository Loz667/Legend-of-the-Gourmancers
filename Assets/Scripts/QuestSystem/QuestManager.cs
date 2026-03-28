using LotG.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace LotG.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        private static GameObject instance;

        private Dictionary<string, Quest> questMap;

        private PartyManager partyManager;

        private int currentPlayerLevel;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this.gameObject;
            }
            DontDestroyOnLoad(gameObject);

            questMap = CreateQuestMap();
            Debug.Log("Quest data loaded");
        }

        private void OnEnable()
        {
            GameEventsManager.instance.questEvents.OnQuestStarted += StartQuest;
            GameEventsManager.instance.questEvents.OnAdvanceQuest += AdvanceQuest;
            GameEventsManager.instance.questEvents.OnQuestCompleted += CompleteQuest;

            GameEventsManager.instance.questEvents.OnQuestStepStateChange += QuestStepStateChange;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.questEvents.OnQuestStarted -= StartQuest;
            GameEventsManager.instance.questEvents.OnAdvanceQuest -= AdvanceQuest;
            GameEventsManager.instance.questEvents.OnQuestCompleted -= CompleteQuest;

            GameEventsManager.instance.questEvents.OnQuestStepStateChange -= QuestStepStateChange;
        }

        private void Start()
        {
            foreach (Quest quest in questMap.Values)
            {
                if (quest.questState == QuestState.IN_PROGRESS)
                {
                    quest.InstantiateCurrentQuestStep(this.transform);
                }
                GameEventsManager.instance.questEvents.QuestStateChange(quest);
            }

            partyManager = FindFirstObjectByType<PartyManager>();
            currentPlayerLevel = partyManager.GetPlayerLevel(); ;
        }

        private void Update()
        {
            foreach (Quest quest in questMap.Values)
            {
                if (quest.questState == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
                {
                    UpdateQuestState(quest.questInfo.QuestId, QuestState.CAN_START);
                }
            }
        }

        public void SaveQuest()
        {
            foreach (Quest quest in questMap.Values)
            {
                SaveQuestData(quest);
            }
        }

        private void UpdateQuestState(string questId, QuestState newState)
        {
            Quest quest = GetQuestFromId(questId);
            quest.questState = newState;
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }

        private bool CheckRequirementsMet(Quest quest)
        {
            bool requirementsMet = true;

            if (currentPlayerLevel < quest.questInfo.requiredLevel)
            {
                requirementsMet = false;
                Debug.Log($"Player level {currentPlayerLevel} is below required level {quest.questInfo.requiredLevel} for quest {quest.questInfo.QuestId}");
            }

            foreach (QuestInfoSO prerequisites in quest.questInfo.questPrerequisites)
            {
                if (GetQuestFromId(prerequisites.QuestId).questState != QuestState.COMPLETED)
                {
                    requirementsMet = false;
                    Debug.Log($"Prerequisite quest {prerequisites.QuestId} is not completed for quest {quest.questInfo.QuestId}");

                    break;
                }
            }

            return requirementsMet;
        }

        private void StartQuest(string questId)
        {
            Quest quest = GetQuestFromId(questId);
            quest.InstantiateCurrentQuestStep(this.transform);
            UpdateQuestState(quest.questInfo.QuestId, QuestState.IN_PROGRESS);
        }

        private void AdvanceQuest(string questId)
        {
            Quest quest = GetQuestFromId(questId);

            quest.MoveToNextQuestStep();

            if (quest.CurrentQuestStepExists())
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            else
            {
                UpdateQuestState(quest.questInfo.QuestId, QuestState.CAN_COMPLETE);
            }
        }

        private void CompleteQuest(string questId)
        {
            Quest quest = GetQuestFromId(questId);
            UpdateQuestState(quest.questInfo.QuestId, QuestState.COMPLETED);
        }

        private void QuestStepStateChange(string questId, int stepIndex, QuestStepState questStepState)
        {
            Quest quest = GetQuestFromId(questId);
            quest.StoreQuestStepState(questStepState, stepIndex);
            UpdateQuestState(questId, quest.questState);
        }

        private Dictionary<string, Quest> CreateQuestMap()
        {
            QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

            Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
            foreach (QuestInfoSO questInfo in allQuests)
            {
                if (idToQuestMap.ContainsKey(questInfo.QuestId))
                {
                    Debug.LogError($"Duplicate quest ID found: {questInfo.QuestId}");
                }
                idToQuestMap.Add(questInfo.QuestId, LoadQuestData(questInfo));
            }
            return idToQuestMap;
        }

        private Quest GetQuestFromId(string questId)
        {
            Quest quest = questMap[questId];
            if (quest == null)
            {
                Debug.LogError($"Quest with ID {questId} not found.");
            }
            return quest;
        }

        private void OnApplicationQuit()
        {
            foreach (Quest quest in questMap.Values)
            {
                SaveQuestData(quest);
            }
        }

        private void SaveQuestData(Quest quest)
        {
            try
            {
                QuestData questData = quest.GetQuestData();
                string serializedData = JsonUtility.ToJson(questData);
                PlayerPrefs.SetString(quest.questInfo.QuestId, serializedData);

                Debug.Log($"Saved quest data for quest {quest.questInfo.QuestId}: {serializedData}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error saving quest data for quest {quest.questInfo.QuestId}: {e}");
            }
        }

        private Quest LoadQuestData(QuestInfoSO questInfo)
        {
            Quest quest = null;
            try
            {
                if (PlayerPrefs.HasKey(questInfo.QuestId))
                {
                    string serializedData = PlayerPrefs.GetString(questInfo.QuestId);
                    QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                    quest = new Quest(questInfo, questData.questState, questData.questStepIndex, questData.questStepStates);
                }
                else
                {
                    quest = new Quest(questInfo);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading quest data for quest {questInfo.QuestId}: {e}");
            }
            return quest;
        }
    }
}

using System;
using UnityEngine;
using LotG.Events;
using LotG.Input;

namespace LotG.QuestSystem
{
    [RequireComponent(typeof(SphereCollider))]
    public class QuestPoint : MonoBehaviour
    {
        [Header("Dialogue (optional)")]
        [SerializeField] private string dialogueKnotName;

        [Header("Quest")]
        [SerializeField] private QuestInfoSO questInfoForPoint;

        [Header("Config")]
        [SerializeField] private bool startQuestPoint;
        [SerializeField] private bool completeQuestPoint;

        private string questId;
        private QuestState currentQuestState;
        private QuestIcon questIcon;

        private bool playerIsNearby;

        private void Awake()
        {
            questId = questInfoForPoint.QuestId;
            questIcon = GetComponentInChildren<QuestIcon>();
        }

        private void OnEnable()
        {
            GameEventsManager.instance.questEvents.OnQuestStateChange += UpdateQuestState;
            GameEventsManager.instance.inputEvents.OnSubmitPressed += HandleSubmitPressed;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.questEvents.OnQuestStateChange -= UpdateQuestState;
            GameEventsManager.instance.inputEvents.OnSubmitPressed -= HandleSubmitPressed;
        }

        private void UpdateQuestState(Quest quest)
        {
            if (quest.questInfo.QuestId.Equals(questId))
            {
                currentQuestState = quest.questState;
                questIcon.SetState(currentQuestState, startQuestPoint, completeQuestPoint);
            }
        }

        private void HandleSubmitPressed(InputEventContext inputEventContext)
        {
            if (!playerIsNearby || !inputEventContext.Equals(InputEventContext.DEFAULT)) return;

            if (!dialogueKnotName.Equals(""))
            {
                GameEventsManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
            }
            else
            {
                if (currentQuestState.Equals(QuestState.CAN_START) && startQuestPoint)
                {
                    GameEventsManager.instance.questEvents.StartQuest(questId);
                }
                else if (currentQuestState.Equals(QuestState.CAN_COMPLETE) && completeQuestPoint)
                {
                    GameEventsManager.instance.questEvents.CompleteQuest(questId);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsNearby = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsNearby = false;
            }
        }
    }
}

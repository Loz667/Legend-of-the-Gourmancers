using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using LotG.QuestSystem;

namespace LotG.UI
{
    public class QuestLogButton : MonoBehaviour, ISelectHandler
    {
        public Button button { get; private set; }
        private TextMeshProUGUI buttonText;
        private UnityAction onSelectAction;

        public void Initialize(string text, UnityAction selectAction)
        {
            this.button = GetComponent<Button>();
            this.buttonText = GetComponentInChildren<TextMeshProUGUI>();
            this.buttonText.text = text;
            this.onSelectAction = selectAction;
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelectAction();
        }

        public void SetState(QuestState state)
        {
            switch (state)
            {
                case QuestState.REQUIREMENTS_NOT_MET:
                case QuestState.CAN_START:
                    button.interactable = false;
                    buttonText.color = Color.red;
                    break;
                case QuestState.IN_PROGRESS:
                case QuestState.CAN_COMPLETE:
                    button.interactable = true;
                    buttonText.color = Color.yellow;
                    break;
                case QuestState.COMPLETED:
                    button.interactable = false;
                    buttonText.color = Color.green;
                    break;
                default:
                    Debug.LogWarning("Unknown QuestState: " + state);
                    break;
            }
        }
    }
}

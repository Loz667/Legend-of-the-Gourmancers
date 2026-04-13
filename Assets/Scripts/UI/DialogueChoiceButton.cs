using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using LotG.Events;

namespace LotG.UI
{
    public class DialogueChoiceButton : MonoBehaviour, ISelectHandler
    {
        [Header("UI Elements")]
        [SerializeField] private Button choiceButton;
        [SerializeField] private TextMeshProUGUI choiceText;

        private int choiceIndex = -1;

        public void SetChoiceText(string choiceTextString)
        {
            choiceText.text = choiceTextString;
        }

        public void SetChoiceIndex(int choiceIndex)
        {
            this.choiceIndex = choiceIndex;
        }

        public void SelectButton()
        {
            choiceButton.Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            GameEventsManager.instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
        }
    }
}

using Ink.Runtime;
using LotG.Events;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LotG.UI
{
    public class DialoguePanelUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private DialogueChoiceButton[] dialogueChoiceButtons;

        private void Awake()
        {
            dialoguePanel.SetActive(false);
            ResetPanel();
        }

        private void OnEnable()
        {
            GameEventsManager.instance.dialogueEvents.OnDialogueStarted += DialogueStarted;
            GameEventsManager.instance.dialogueEvents.OnDialogueFinished += DialogueFinished;
            GameEventsManager.instance.dialogueEvents.OnDisplayDialogue += DisplayDialogue;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.dialogueEvents.OnDialogueStarted -= DialogueStarted;
            GameEventsManager.instance.dialogueEvents.OnDialogueFinished -= DialogueFinished;
            GameEventsManager.instance.dialogueEvents.OnDisplayDialogue -= DisplayDialogue;
        }

        private void DialogueStarted()
        {
            dialoguePanel.SetActive(true);
        }

        private void DialogueFinished()
        {
            dialoguePanel.SetActive(false);
            ResetPanel();
        }

        private void DisplayDialogue(string dialogue, List<Choice> dialogueChoices)
        {
            dialogueText.text = dialogue;

            if (dialogueChoices.Count > dialogueChoiceButtons.Length)
            {
                Debug.LogError("Not enough dialogue choice buttons to display all choices.");
                return;
            }

            foreach (DialogueChoiceButton button in dialogueChoiceButtons)
            {
                button.gameObject.SetActive(false);
            }

            int buttonIndex = dialogueChoices.Count - 1;
            for (int inkchoiceIndex = 0; inkchoiceIndex < dialogueChoices.Count; inkchoiceIndex++)
            {
                Choice dialogueChoice = dialogueChoices[inkchoiceIndex];
                DialogueChoiceButton button = dialogueChoiceButtons[buttonIndex];

                button.SetChoiceText(dialogueChoice.text);
                button.SetChoiceIndex(inkchoiceIndex);
                button.gameObject.SetActive(true);

                if (inkchoiceIndex == 0)
                {
                    button.SelectButton();
                    GameEventsManager.instance.dialogueEvents.UpdateChoiceIndex(0);
                }

                buttonIndex--;
            }
        }

        private void ResetPanel()
        {
            dialogueText.text = "";
        }
    }
}

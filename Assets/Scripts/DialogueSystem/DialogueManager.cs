using UnityEngine;
using LotG.Events;
using LotG.Input;
using Ink.Runtime;

namespace LotG.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("Dialogue File")]
        [SerializeField] private TextAsset inkJSON;

        private Story currentStory;
        private InkExternalFunctions inkExternalFunctions;

        private int currentChoiceIndex = -1;

        bool dialoguePlaying = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                currentStory = new Story(inkJSON.text);
                inkExternalFunctions = new InkExternalFunctions();
                inkExternalFunctions.Bind(currentStory);
            }
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            inkExternalFunctions.Unbind(currentStory);
        }

        void OnEnable()
        {
            GameEventsManager.instance.dialogueEvents.OnEnterDialogue += EnterDialogue;
            GameEventsManager.instance.inputEvents.OnSubmitPressed += SubmitPressed;
            GameEventsManager.instance.dialogueEvents.OnUpdateChoiceIndex += UpdateChoiceIndex;
        }

        void OnDisable()
        {
            GameEventsManager.instance.dialogueEvents.OnEnterDialogue -= EnterDialogue;
            GameEventsManager.instance.inputEvents.OnSubmitPressed -= SubmitPressed;
            GameEventsManager.instance.dialogueEvents.OnUpdateChoiceIndex -= UpdateChoiceIndex;
        }

        void UpdateChoiceIndex(int choiceIndex)
        {
            this.currentChoiceIndex = choiceIndex;
        }

        void EnterDialogue(string dialogue)
        {
            if (dialoguePlaying) return;

            dialoguePlaying = true;

            GameEventsManager.instance.dialogueEvents.DialogueStarted();

            GameEventsManager.instance.miscEvents.DisablePlayerMovement();

            GameEventsManager.instance.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);

            if (!dialogue.Equals(""))
            {
                currentStory.ChoosePathString(dialogue);
            }
            else
            {
                Debug.LogWarning("Dialogue string is empty. Please provide a valid dialogue path.");
            }

            ContinueOrExitStory();
        }

        void SubmitPressed(InputEventContext inputEventContext)
        {
            if (!inputEventContext.Equals(InputEventContext.DIALOGUE)) return;

            ContinueOrExitStory();
        }

        void ContinueOrExitStory()
        {
            if (currentStory.currentChoices.Count > 0 && currentChoiceIndex != -1)
            {
                currentStory.ChooseChoiceIndex(currentChoiceIndex);
                currentChoiceIndex = -1;
            }

            if (currentStory.canContinue)
            {
                string dialogue = currentStory.Continue();

                while (IsLineBlank(dialogue) && currentStory.canContinue)
                {
                    dialogue = currentStory.Continue();
                }

                if (IsLineBlank(dialogue) && !currentStory.canContinue)
                {
                    ExitDialogue();
                }
                else
                {
                    GameEventsManager.instance.dialogueEvents.DisplayDialogue(dialogue, currentStory.currentChoices);
                }
            }
            else if (currentStory.currentChoices.Count == 0)
            {
                ExitDialogue();
            }
        }

        void ExitDialogue()
        {
            dialoguePlaying = false;

            GameEventsManager.instance.dialogueEvents.DialogueFinished();

            GameEventsManager.instance.miscEvents.EnablePlayerMovement();

            GameEventsManager.instance.inputEvents.ChangeInputEventContext(InputEventContext.DEFAULT);

            currentStory.ResetState();
        }

        private bool IsLineBlank(string line)
        {
            return line.Trim().Equals("") || line.Trim().Equals("\n");
        }
    }
}

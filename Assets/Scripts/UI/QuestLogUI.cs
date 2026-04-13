using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LotG.QuestSystem;
using LotG.Events;

namespace LotG.UI
{
    public class QuestLogUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private QuestListUI questListUI;
        [SerializeField] private GameObject questDescriptionPanel;
        [SerializeField] private TextMeshProUGUI questNameText;
        [SerializeField] private TextMeshProUGUI questStatusText;

        private Button firstSelected;

        private void OnEnable()
        {
            GameEventsManager.instance.questEvents.OnQuestStateChange += QuestStateChange;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.questEvents.OnQuestStateChange -= QuestStateChange;
        }

        private void QuestStateChange(Quest quest)
        {
            QuestLogButton questLogButton = questListUI.CreateButtonIfNotExists(quest, () => { SetQuestLogInfo(quest); });

            if (firstSelected == null)
            {
                firstSelected = questLogButton.button;
                firstSelected.Select();
            }

            questLogButton.SetState(quest.questState);
        }

        private void SetQuestLogInfo(Quest quest)
        {
            questNameText.text = quest.questInfo.questName;

            questStatusText.text = quest.GetFullStatusText();
        }

    }
}

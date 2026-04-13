using LotG.QuestSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LotG.UI
{
    public class QuestListUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject questListPanel;
        [SerializeField] private GameObject questLogButtonPrefab;
        [SerializeField] private RectTransform scrollRectTransform;
        [SerializeField] private RectTransform contentRectTransform;

        private Dictionary<string, QuestLogButton> idToButtonMap = new Dictionary<string, QuestLogButton>();

        public QuestLogButton CreateButtonIfNotExists(Quest quest, UnityAction selectAction)
        {
            QuestLogButton questLogButton = null;

            if (!idToButtonMap.ContainsKey(quest.questInfo.QuestId))
            {
                questLogButton = InstantiateButtonForQuest(quest, selectAction);
            }
            else
            {
                questLogButton = idToButtonMap[quest.questInfo.QuestId];
            }
            return questLogButton;
        }

        private QuestLogButton InstantiateButtonForQuest(Quest quest, UnityAction selectAction)
        {
            QuestLogButton questLogButton = Instantiate(questLogButtonPrefab, questListPanel.transform).GetComponent<QuestLogButton>();
            questLogButton.gameObject.name = quest.questInfo.QuestId + " Button";
            RectTransform buttonRectT = questLogButton.GetComponent<RectTransform>();
            questLogButton.Initialize(quest.questInfo.questName, () => {
                selectAction();
                UpdateScrolling(buttonRectT);
                });
            idToButtonMap[quest.questInfo.QuestId] = questLogButton;
            return questLogButton;
        }

        private void UpdateScrolling(RectTransform buttonRectTransform)
        {
            float buttonYMin = Mathf.Abs(buttonRectTransform.anchoredPosition.y);
            float buttonYMax = buttonYMin + buttonRectTransform.rect.height;

            float contentYMin = contentRectTransform.anchoredPosition.y;
            float contentYMax = contentYMin + scrollRectTransform.rect.height;

            if (buttonYMax > contentYMax)
            {
                contentRectTransform.anchoredPosition += new Vector2(contentRectTransform.anchoredPosition.x, buttonYMax - contentYMax);
            }
            else if (buttonYMin < contentYMin)
            {
                contentRectTransform.anchoredPosition -= new Vector2(contentRectTransform.anchoredPosition.x, buttonYMin);
            }
        }
    }
}

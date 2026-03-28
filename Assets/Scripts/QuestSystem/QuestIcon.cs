using UnityEngine;

namespace LotG.QuestSystem
{
    public class QuestIcon : MonoBehaviour
    {
        [Header("Icons")]
        [SerializeField] private GameObject requirementsNotMetIcon;
        [SerializeField] private GameObject canStartOrCompleteIcon;
        [SerializeField] private GameObject inProgressIcon;

        public void SetState(QuestState newState, bool startPoint, bool completePoint)
        {
            requirementsNotMetIcon.SetActive(false);
            canStartOrCompleteIcon.SetActive(false);
            inProgressIcon.SetActive(false);

            switch (newState)
            {
                case QuestState.REQUIREMENTS_NOT_MET:
                    if (startPoint) requirementsNotMetIcon.SetActive(true);
                    break;
                case QuestState.CAN_START:
                    if (startPoint) canStartOrCompleteIcon.SetActive(true);
                    break;
                case QuestState.IN_PROGRESS:
                    if (startPoint) inProgressIcon.SetActive(true);
                    break;
                case QuestState.CAN_COMPLETE:
                    if (completePoint) canStartOrCompleteIcon.SetActive(true);
                    break;
                case QuestState.COMPLETED:
                    break;
                default:
                    Debug.LogWarning(newState + " is not a valid quest state for a quest icon.");
                    break;
            }
        }
    }
}

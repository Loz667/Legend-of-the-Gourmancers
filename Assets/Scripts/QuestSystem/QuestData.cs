

namespace LotG.QuestSystem
{
    [System.Serializable]
    public class QuestData
    {
        public QuestState questState;
        public int questStepIndex;
        public QuestStepState[] questStepStates;

        public QuestData(QuestState state, int stepIndex, QuestStepState[] stepStates)
        {
            this.questState = state;
            this.questStepIndex = stepIndex;
            this.questStepStates = stepStates;
        }
    }
}

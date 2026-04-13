

namespace LotG.QuestSystem
{
    [System.Serializable]
    public class QuestStepState
    {
        public string stepState;
        public string stepStatus;

        public QuestStepState(string stepState, string stepStatus)
        {
            this.stepState = stepState;
            this.stepStatus = stepStatus;
        }

        public QuestStepState()
        {
            this.stepState = "";
            this.stepStatus = "";
        }
    }
}

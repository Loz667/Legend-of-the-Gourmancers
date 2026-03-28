

namespace LotG.QuestSystem
{
    [System.Serializable]
    public class QuestStepState
    {
        public string stepState;

        public QuestStepState(string stepState)
        {
            this.stepState = stepState;
        }

        public QuestStepState()
        {
            this.stepState = "";
        }
    }
}

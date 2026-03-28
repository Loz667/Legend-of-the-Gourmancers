using UnityEngine;

namespace LotG.QuestSystem
{
    [CreateAssetMenu(fileName = "New Quest Info", menuName = "Scriptable Objects/Quest System/Create Quest Info")]
    public class QuestInfoSO : ScriptableObject
    {
        [field: SerializeField] public string QuestId { get; private set; }

        [Header("Quest Info")]
        public string questName;

        [Header("Quest Requirements")]
        public int requiredLevel;
        public QuestInfoSO[] questPrerequisites;

        [Header("Quest Steps")]
        public GameObject[] questStepPrefabs;

        [Header("Quest Rewards")]
        public int experienceReward;

        private void OnValidate()
        {
            #if UNITY_EDITOR
            QuestId = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}

using UnityEngine;

namespace LotG.Events
{
    public class GameEventsManager : MonoBehaviour
    {
        public static GameEventsManager instance { get; private set; }

        public DialogueEvents dialogueEvents;
        public InputEvents inputEvents;
        public QuestEvents questEvents;
        public MiscEvents miscEvents;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(gameObject);

            dialogueEvents = new DialogueEvents();
            inputEvents = new InputEvents();
            questEvents = new QuestEvents();
            miscEvents = new MiscEvents();
        }
    }
}


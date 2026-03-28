using LotG.QuestSystem;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

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

        inputEvents = new InputEvents();
        questEvents = new QuestEvents();
        miscEvents = new MiscEvents();
    }
}

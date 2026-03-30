using Ink.Runtime;
using LotG.Events;

public class InkExternalFunctions
{
    public void Bind(Story story)
    {
        story.BindExternalFunction("StartQuest", (string questName) => StartQuest(questName));
        story.BindExternalFunction("AdvanceQuest", (string questName) => AdvanceQuest(questName));
        story.BindExternalFunction("CompleteQuest", (string questName) => CompleteQuest(questName));
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("StartQuest");
        story.UnbindExternalFunction("AdvanceQuest");
        story.UnbindExternalFunction("CompleteQuest");
    }

    private void StartQuest(string questName)
    {
        GameEventsManager.instance.questEvents.StartQuest(questName);
    }

    private void AdvanceQuest(string questName)
    {
        GameEventsManager.instance.questEvents.AdvanceQuest(questName);
    }

    private void CompleteQuest(string questName)
    {
        GameEventsManager.instance.questEvents.CompleteQuest(questName);
    }
}

EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)

->DefeatSlimeQuest

=== DefeatSlimeQuest ===
Spud! Slimes are attacking the village and eating all the fresh produce! Can you help?
* [Yes]
    ~StartQuest("DefeatSlimeQuest")
    Great! Head to the market to see if they have anything that you can use.
* [No]
    Oh...
--> END
using UnityEngine;

[CreateAssetMenu(fileName = "New Party Member Info", menuName = "Scriptable Objects/Battle System/Create Party Member", order = 1)]
public class PartyMemberInfoSO : ScriptableObject
{
    public string MemberName;
    public int StartingLevel;
    public int BaseHealth;
    public int BaseInitiative;
    public GameObject BattleVisualPrefab;
    public GameObject OverworldVisualPrefab;
}

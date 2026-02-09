using UnityEngine;

[CreateAssetMenu(fileName = "New Party Member", menuName = "Battle System/Party Member")]
public class PartyMemberInfo : ScriptableObject
{
    public string MemberName;
    public int StartingLevel;
    public int BaseHealth;
    public int BaseInitiative;
    public GameObject BattleVisualPrefab;
    public GameObject OverworldVisualPrefab;
}

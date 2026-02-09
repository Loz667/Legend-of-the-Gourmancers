using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private PartyMemberInfo[] allMembers;
    [SerializeField] private List<PartyMember> currentPartyMembers;
    [SerializeField] private PartyMemberInfo defaultPartyMember;

    private void Awake()
    {
        AddPartyMemberByName(defaultPartyMember.MemberName);
    }

    public void AddPartyMemberByName(string memberName)
    {
        for (int i = 0; i < allMembers.Length; i++)
        {
            if (allMembers[i].MemberName == memberName)
            {
                PartyMember newPartyMember = new PartyMember();
                newPartyMember.MemberName = allMembers[i].MemberName;
                newPartyMember.Level = allMembers[i].StartingLevel;
                newPartyMember.CurrHealth = allMembers[i].BaseHealth;
                newPartyMember.MaxHealth = newPartyMember.CurrHealth;
                newPartyMember.Initiative = allMembers[i].BaseInitiative;
                newPartyMember.BattleVisualPrefab = allMembers[i].BattleVisualPrefab;
                newPartyMember.OverworldVisualPrefab = allMembers[i].OverworldVisualPrefab;

                currentPartyMembers.Add(newPartyMember);
            }
        }
    }
}

[System.Serializable]
public class PartyMember
{
    public string MemberName;
    public int Level;
    public int CurrHealth;
    public int MaxHealth;
    public int Initiative;
    public int CurrXP;
    public int MaxXP;
    public GameObject BattleVisualPrefab;
    public GameObject OverworldVisualPrefab;
}

using System.Collections.Generic;
using UnityEngine;

namespace LotG.Battle
{
    public class PartyManager : MonoBehaviour
    {
        private static GameObject instance;

        [SerializeField] private PartyMemberInfoSO[] allMembers;
        [SerializeField] private List<PartyMember> currentPartyMembers;
        [SerializeField] private PartyMemberInfoSO defaultPartyMember;

        private Vector3 playerPosition;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this.gameObject;
                AddPartyMemberByName(defaultPartyMember.MemberName);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ResetHealth();
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

        public List<PartyMember> GetCurrentPartyMembers()
        {
            return currentPartyMembers;
        }

        public void SaveHealth(int partyMember, int health)
        {
            currentPartyMembers[partyMember].CurrHealth = health;
        }

        public void SetPosition(Vector3 currentPosition)
        {
            playerPosition = currentPosition;
        }

        public Vector3 GetPosition()
        {
            return playerPosition;
        }

        public int GetPlayerLevel()
        {
            return currentPartyMembers[0].Level;
        }

        private void ResetHealth()
        {
            for (int i = 0; i < currentPartyMembers.Count; i++)
            {
                currentPartyMembers[i].CurrHealth = currentPartyMembers[i].MaxHealth;
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
}

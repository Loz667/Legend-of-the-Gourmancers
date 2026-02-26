using LotG.Inventories;
using LotG.UI;
using LotG.UI.Battle;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LotG.Battle
{
    public class BattleSystem : MonoBehaviour
    {
        private enum BattleState { Start, Selection, Battle, Won, Lost, Fled }

        [Header("Battle State")]
        [SerializeField] private BattleState currentState;

        [Header("Combatants")]
        [SerializeField] private List<BattleEntity> allCombatants = new List<BattleEntity>();
        [SerializeField] private List<BattleEntity> playerCombatants = new List<BattleEntity>();
        [SerializeField] private List<BattleEntity> enemyCombatants = new List<BattleEntity>();

        [Space(10)]
        [Header("Battle Spawn Points")]
        [SerializeField] private Transform[] partySpawnPoints;
        [SerializeField] private Transform[] enemySpawnPoints;

        [Space(10)]
        [Header("Battle UI")]
        [SerializeField] private GameObject battleMenu;
        [SerializeField] private TextMeshProUGUI actionText;
        [SerializeField] private GameObject enemySelectMenu;
        [SerializeField] private GameObject[] enemySelectButtons;
        [SerializeField] private GameObject itemSelectMenu;
        [SerializeField] private ItemInventoryUI itemSelectUI;
        [SerializeField] private GameObject battleTextPopUp;
        [SerializeField] private TextMeshProUGUI battleText;

        [Space(10)]
        [Header("Inventory")]
        [SerializeField] private InventorySO inventoryData;

        private PartyManager partyManager;
        private EnemyManager enemyManager;
        private int currentPlayer;
        protected int mealHealthValue;

        private const string ACTION_MSG = "'s Actions";
        private const string WIN_MSG = "You won the battle!";
        private const int TURN_DURATION = 2;

        private void Start()
        {
            partyManager = FindFirstObjectByType<PartyManager>();
            enemyManager = FindFirstObjectByType<EnemyManager>();

            itemSelectUI.Initialize(inventoryData.inventorySize);
            itemSelectUI.OnItemActionRequsted += RemoveSelectedItem;

            CreatePartyEntities();
            CreateEnemyEntities();
            ShowBattleMenu();
        }

        private IEnumerator BattleRoutine()
        {
            itemSelectMenu.SetActive(false);
            currentState = BattleState.Battle;
            battleTextPopUp.SetActive(true);

            for (int i = 0; i < allCombatants.Count; i++)
            {
                switch (allCombatants[i].BattleAction)
                {
                    case BattleEntity.Action.Feed:
                        yield return StartCoroutine(FeedRoutine(i));
                        //FeedAction(allCombatants[i], allCombatants[allCombatants[i].Target]);
                        break;
                    case BattleEntity.Action.Flee:
                        //AttackAction(allCombatants[i], allCombatants[allCombatants[i].Target]);
                        break;
                    default:
                        Debug.Log("Error: No Battle Action Selected");
                        break;
                }
            }

            if(currentState == BattleState.Battle)
            {
                battleTextPopUp.SetActive(false);
                currentPlayer = 0;
                ShowBattleMenu();
            }

            yield return null;
        }

        private IEnumerator FeedRoutine(int i)
        {
            if (allCombatants[i].IsPlayer == true)
            {
                //Feed selected enemy
                BattleEntity currAttacker = allCombatants[i];
                if (allCombatants[currAttacker.Target].IsPlayer || currAttacker.Target >= allCombatants.Count)
                {
                    currAttacker.SetTarget(GetRandomEnemy());
                }
                BattleEntity currTarget = allCombatants[currAttacker.Target];
                FeedAction(currAttacker, currTarget);

                yield return new WaitForSeconds(TURN_DURATION);

                if (currTarget.CurrHealth >= currTarget.MaxHealth)
                {
                    battleText.text = string.Format("{0} reached full hunger", currTarget.Name);
                    yield return new WaitForSeconds(TURN_DURATION);
                    currTarget.BattleVisual.PlayDeathAnim();
                    enemyCombatants.Remove(currTarget);
                    allCombatants.Remove(currTarget);

                    if (enemyCombatants.Count <= 0)
                    {
                        currentState = BattleState.Won;
                        battleText.text = WIN_MSG;
                        yield return new WaitForSeconds(TURN_DURATION);
                        //SceneManager.LoadSceneAsync(OVERWORLD_SCENE);
                    }
                }
            }
        }

        private void CreatePartyEntities()
        {
            List<PartyMember> partyMembers = new List<PartyMember>();
            partyMembers = partyManager.GetCurrentPartyMembers();

            for (int i = 0; i < partyMembers.Count; i++)
            {
                BattleEntity tempEntity = new BattleEntity();
                tempEntity.SetEntityValues(partyMembers[i].MemberName, partyMembers[i].Level,
                    partyMembers[i].CurrHealth, partyMembers[i].MaxHealth,
                    partyMembers[i].Initiative, 0, true);

                BattleVisual tempBattleVis = Instantiate(partyMembers[i].BattleVisualPrefab,
                    partySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisual>();

                tempBattleVis.SetStartingValues(partyMembers[i].MaxHealth, partyMembers[i].MaxHealth,
                    partyMembers[i].Level);
                tempEntity.BattleVisual = tempBattleVis;

                allCombatants.Add(tempEntity);
                playerCombatants.Add(tempEntity);
            }
        }

        private void CreateEnemyEntities()
        {
            List<Enemy> currentEnemies = new List<Enemy>();
            currentEnemies = enemyManager.GetCurrentEnemies();

            for (int i = 0; i < currentEnemies.Count; i++)
            {
                BattleEntity tempEntity = new BattleEntity();
                tempEntity.SetEntityValues(currentEnemies[i].EnemyName, currentEnemies[i].Level,
                    currentEnemies[i].CurrHunger, currentEnemies[i].MaxHunger,
                    currentEnemies[i].Initiative, currentEnemies[i].Strength, false);

                BattleVisual tempBattleVis = Instantiate(currentEnemies[i].BattleVisualPrefab,
                    enemySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisual>();

                tempBattleVis.SetStartingValues(currentEnemies[i].CurrHunger, currentEnemies[i].MaxHunger,
                    currentEnemies[i].Level);
                tempEntity.BattleVisual = tempBattleVis;

                allCombatants.Add(tempEntity);
                enemyCombatants.Add(tempEntity);
            }
        }

        public void ShowBattleMenu()
        {
            actionText.text = playerCombatants[currentPlayer].Name + ACTION_MSG;
            battleMenu.SetActive(true);
        }

        public void ShowEnemySelectMenu()
        {
            battleMenu.SetActive(false);
            SetEnemySelectButtons();
            enemySelectMenu.SetActive(true);
        }

        private void SetEnemySelectButtons()
        {
            for (int i = 0; i < enemySelectButtons.Length; i++)
            {
                enemySelectButtons[i].SetActive(false);
            }

            for (int j = 0; j < enemyCombatants.Count; j++)
            {
                enemySelectButtons[j].SetActive(true);
                enemySelectButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = enemyCombatants[j].Name;
            }
        }

        public void ShowItemSelectMenu()
        {
            enemySelectMenu.SetActive(false);
            SetItemSelectButtons();
            itemSelectMenu.SetActive(true);
        }

        private void SetItemSelectButtons()
        {
            itemSelectUI.ResetAllItems();
            foreach (var item in inventoryData.GetCurrentState())
            {
                itemSelectUI.UpdateData(item.Key,
                    item.Value.item.GetIcon(),
                    item.Value.quantity,
                    item.Value.item.GetHealthRestoreValue());
            }
        }

        public void SelectEnemy(int currentEnemy)
        {
            BattleEntity currentPlayerEntity = playerCombatants[currentPlayer];
            currentPlayerEntity.SetTarget(allCombatants.IndexOf(enemyCombatants[currentEnemy]));
        }

        public void SelectItem(int value)
        {
            BattleEntity currentPlayerEntity = playerCombatants[currentPlayer];
            currentPlayerEntity.BattleAction = BattleEntity.Action.Feed;
            mealHealthValue = value;

            //foreach(var item in inventoryData.GetCurrentState())
            //{
            //    inventoryData.RemoveItem(item.Value.item);
            //    //itemSelectUI.ResetData(item.Key);
            //}

            currentPlayer++;

            if (currentPlayer >= playerCombatants.Count)
            {
                //Start Battle
                StartCoroutine(BattleRoutine());
            }
            else
            {
                itemSelectMenu.SetActive(false);
                ShowBattleMenu();
            }
        }

        private void RemoveSelectedItem(int itemIndex)
        {
            inventoryData.RemoveItem(inventoryData.GetCurrentState()[itemIndex].item);
            itemSelectUI.ResetData(itemIndex);
        }

        private void FeedAction(BattleEntity currAttacker, BattleEntity currTarget)
        {
            currAttacker.BattleVisual.PlayAttackAnim();
            currTarget.CurrHealth += mealHealthValue;
            currTarget.BattleVisual.PlayHitAnim();
            currTarget.UpdateHealthUI();
            battleText.text = string.Format("{0} fed {1} for {2} health!", currAttacker.Name, currTarget.Name, mealHealthValue);
        }

        private void AttackAction(BattleEntity currAttacker, BattleEntity currTarget)
        {
            int damage = currAttacker.Strength;
            currAttacker.BattleVisual.PlayAttackAnim();
            currTarget.CurrHealth -= damage;
            currTarget.BattleVisual.PlayHitAnim();
            currTarget.UpdateHealthUI();
        }

        private int GetRandomPartyMember()
        {
            //Create a temporary list
            List<int> partyMembers = new List<int>();
            //find all party members and add to list
            for (int i = 0; i < allCombatants.Count; i++)
            {
                if (allCombatants[i].IsPlayer == true)
                {
                    partyMembers.Add(i);
                }
            }
            //Return random party member
            return partyMembers[Random.Range(0, partyMembers.Count)];
        }

        private int GetRandomEnemy()
        {
            List<int> enemies = new List<int>();

            for (int i = 0; i < allCombatants.Count; i++)
            {
                if (allCombatants[i].IsPlayer == false)
                {
                    enemies.Add(i);
                }
            }

            return enemies[Random.Range(0, enemies.Count)];
        }
    }

    [System.Serializable]
    public class BattleEntity
    {
        public enum Action { Feed, Flee }
        public Action BattleAction;

        public string Name;
        public int Level;
        public int CurrHealth;
        public int MaxHealth;
        public int Initiative;
        public int Strength;
        public bool IsPlayer;
        public BattleVisual BattleVisual;
        public int Target;

        public void SetEntityValues(string name, int level, int currHealth, int maxHealth, int initiative,
            int strength, bool isPlayer)
        {
            Name = name;
            Level = level;
            CurrHealth = currHealth;
            MaxHealth = maxHealth;
            Initiative = initiative;
            Strength = strength;
            IsPlayer = isPlayer;
        }

        public void SetTarget(int target)
        {
            Target = target;
        }

        public void UpdateHealthUI()
        {
            if (IsPlayer)
                BattleVisual.AdjustPlayerHealth(CurrHealth);
            else
                BattleVisual.AdjustEnemyHealth(CurrHealth);
        }
    }
}
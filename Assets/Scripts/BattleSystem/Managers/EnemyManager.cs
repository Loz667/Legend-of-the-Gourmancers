using System.Collections.Generic;
using UnityEngine;

namespace LotG.Battle
{
    public class EnemyManager : MonoBehaviour
    {
        private static GameObject instance;

        [SerializeField] private EnemyInfoSO[] allEnemies;
        [SerializeField] private List<Enemy> currentEnemies;

        private const float LEVEL_MODIFIER = 0.5f;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this.gameObject;
            }

            DontDestroyOnLoad(gameObject);
        }

        public void GenerateEnemiesPerEncounter(Encounter[] encounters, int maxEnemyCount)
        {
            currentEnemies.Clear();
            int enemyCount = Random.Range(1, maxEnemyCount + 1);

            for (int i = 0; i < enemyCount; i++)
            {
                Encounter tempEncounter = encounters[Random.Range(0, encounters.Length)];
                int level = Random.Range(tempEncounter.LevelMin, tempEncounter.LevelMax + 1);
                GenerateEnemyByName(tempEncounter.Enemy.EnemyName, level);
            }
        }

        private void GenerateEnemyByName(string enemyName, int level)
        {
            for (int i = 0; i < allEnemies.Length; i++)
            {
                if (enemyName == allEnemies[i].EnemyName)
                {
                    Enemy newEnemy = new Enemy();
                    newEnemy.EnemyName = allEnemies[i].EnemyName;
                    newEnemy.Level = level;
                    float levelModifier = (LEVEL_MODIFIER * newEnemy.Level);

                    newEnemy.MaxHunger = Mathf.RoundToInt(allEnemies[i].BaseHunger + (allEnemies[i].BaseHunger + levelModifier));
                    newEnemy.CurrHunger = 0;
                    newEnemy.Initiative = Mathf.RoundToInt(allEnemies[i].BaseInitiative + (allEnemies[i].BaseInitiative + levelModifier));
                    newEnemy.Strength = Mathf.RoundToInt(allEnemies[i].BaseStrength + (allEnemies[i].BaseStrength + levelModifier));
                    newEnemy.BattleVisualPrefab = allEnemies[i].BattleVisualPrefab;

                    currentEnemies.Add(newEnemy);
                }
            }
        }

        public List<Enemy> GetCurrentEnemies()
        {
            return currentEnemies;
        }
    }

    [System.Serializable]
    public class Enemy
    {
        public string EnemyName;
        public int Level;
        public int CurrHunger;
        public int MaxHunger;
        public int Initiative;
        public int Strength;
        public GameObject BattleVisualPrefab;
    }
}

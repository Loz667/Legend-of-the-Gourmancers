using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] allEnemies;
    [SerializeField] private List<Enemy> currentEnemies;

    private const float LEVEL_MODIFIER = 0.5f;

    private void Awake()
    {
        GenerateEnemyByName("Slime", 1);
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
                newEnemy.CurrHunger = allEnemies[i].BaseHunger;
                newEnemy.Initiative = Mathf.RoundToInt(allEnemies[i].BaseInitiative + (allEnemies[i].BaseInitiative + levelModifier));
                newEnemy.BattleVisualPrefab = allEnemies[i].BattleVisualPrefab;

                currentEnemies.Add(newEnemy);
            }
        }
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
    public GameObject BattleVisualPrefab;
}

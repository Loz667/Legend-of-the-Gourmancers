using LotG.Battle;
using System.Xml;
using UnityEngine;

public class EncounterSystem : MonoBehaviour
{
    [SerializeField] private Encounter[] enemiesInScene;
    [SerializeField] private int maxNumEnemies;

    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = FindFirstObjectByType<EnemyManager>();
        enemyManager.GenerateEnemiesPerEncounter(enemiesInScene, maxNumEnemies);
    }
}

[System.Serializable]
public class Encounter
{
    public EnemyInfoSO Enemy;
    public int LevelMin;
    public int LevelMax;
}

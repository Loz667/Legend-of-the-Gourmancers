using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Battle System/Enemy")]
public class EnemyInfo : ScriptableObject
{
    public string EnemyName;
    public int BaseHunger;
    public int BaseInitiative;
    public GameObject BattleVisualPrefab;
}

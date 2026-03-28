using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Info", menuName = "Scriptable Objects/Battle System/Create Enemy")]
public class EnemyInfoSO : ScriptableObject
{
    public string EnemyName;
    public int BaseHunger;
    public int BaseInitiative;
    public int BaseStrength;
    public GameObject BattleVisualPrefab;
}

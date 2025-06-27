using UnityEngine;

[CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "Scriptable Objects/KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    public string ObjName;
    public Sprite ObjSprite;
    public Transform ObjPrefab;
}

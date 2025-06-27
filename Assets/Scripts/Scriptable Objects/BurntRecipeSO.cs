using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BurntRecipeSO", menuName = "Scriptable Objects/BurntRecipeSO")]
public class BurntRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float timeToBurnt;
}

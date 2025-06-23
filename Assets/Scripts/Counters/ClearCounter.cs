using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(PlayerController player)
    {

    }
}

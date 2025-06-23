using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public override void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            Transform kitchenObjT = Instantiate(kitchenObjectSO.ObjPrefab);
            kitchenObjT.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}

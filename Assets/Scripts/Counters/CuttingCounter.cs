using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjSO;
    
    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            //There is no KitchenObject on this counter
            //so check if player is carrying something
            if (player.HasKitchenObject())
            {
                //The player is carrying a KitchenObject and
                //sets it down on the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //Player is not carrying anything
            }
        }
        else
        {
            //There is a KitchenObject on this counter
            //so check if player is carrying something
            if (player.HasKitchenObject())
            {
                //The player is carrying a KitchenObject
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(PlayerController player)
    {
        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(cutKitchenObjSO, this);
        }
    }
}

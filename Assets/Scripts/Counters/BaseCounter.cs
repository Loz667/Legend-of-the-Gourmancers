using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopT;

    private KitchenObject _kitchenObject;
    
    public virtual void Interact(PlayerController player)
    {
        Debug.LogError("BaseCounter.Interact()");
    }
    
    public virtual void InteractAlternate(PlayerController player)
    {
        Debug.LogError("BaseCounter.InteractAlternate()");
    }
    
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopT;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this._kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}

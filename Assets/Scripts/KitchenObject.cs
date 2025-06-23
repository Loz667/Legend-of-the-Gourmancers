using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjP;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjP)
    {
        if (this._kitchenObjP != null)
        {
            this._kitchenObjP.ClearKitchenObject();
        }

        this._kitchenObjP = kitchenObjP;

        if (kitchenObjP.HasKitchenObject())
        {
            Debug.LogError("KitchenObjectParent already has KitcheObject");
        }

        kitchenObjP.SetKitchenObject(this);

        transform.parent = kitchenObjP.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public void DestroySelf()
    {
        _kitchenObjP.ClearKitchenObject();

        Destroy(gameObject);
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjP;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO objSO, IKitchenObjectParent ObjP)
    {
        Transform kitchenObjT = Instantiate(objSO.ObjPrefab);
        KitchenObject obj = kitchenObjT.GetComponent<KitchenObject>();

        obj.SetKitchenObjectParent(ObjP);

        return obj;
    }
}
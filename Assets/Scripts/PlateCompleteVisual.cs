using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject plate;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectGameObjects = new List<KitchenObjectSO_GameObject>();

    private void Start()
    {
        plate.OnIngredientAdded += PlateUpdateVisual;
        
        foreach (KitchenObjectSO_GameObject kitchenObjectGameObject in kitchenObjectGameObjects)
        {
            kitchenObjectGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateUpdateVisual(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectGameObject in kitchenObjectGameObjects)
        {
            if (kitchenObjectGameObject.kitchenObjectSO == e.ingredient)
            {
                kitchenObjectGameObject.gameObject.SetActive(true);
            }
        }
    }
}

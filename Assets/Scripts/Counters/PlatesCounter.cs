using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plate;
    private float _plateSpawnTime;
    private float _plateMaxSpawnTime = 4f;

    private int _plateSpawnedAmount;
    private int _plateSpawnedMax = 4;

    public override void Interact(PlayerController player)
    {
        //Player is not carrying anything
        if (!player.HasKitchenObject())
        {
            //Check there is an available plate
            if (_plateSpawnedAmount > 0)
            {
                //Reduce amount
                _plateSpawnedAmount--;
                
                //Give plate to player
                KitchenObject.SpawnKitchenObject(plate, player);
                
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    

    private void Update()
    {
        _plateSpawnTime += Time.deltaTime;
        if (_plateSpawnTime >= _plateMaxSpawnTime)
        {
            _plateSpawnTime = 0f;
            if (_plateSpawnedAmount < _plateSpawnedMax)
            {
                _plateSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

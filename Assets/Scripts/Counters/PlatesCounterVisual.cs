using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter counter;
    [SerializeField] private Transform counterTopT;
    [SerializeField] private Transform plateVisual;
    
    private List<GameObject> _plates = new List<GameObject>();

    private void Start()
    {
        counter.OnPlateSpawned += UpdateVisual;
        counter.OnPlateRemoved += PlateRemoved;
    }

    private void UpdateVisual(object sender, EventArgs e)
    {
        Transform plate = Instantiate(plateVisual, counterTopT);

        float plateYOffset = 0.1f;
        plate.localPosition = new Vector3(0, plateYOffset * _plates.Count, 0);
        
        _plates.Add(plate.gameObject);
    }

    private void PlateRemoved(object sender, EventArgs e)
    {
        GameObject plate = _plates[_plates.Count - 1];
        _plates.Remove(plate);
        Destroy(plate);
    }
}

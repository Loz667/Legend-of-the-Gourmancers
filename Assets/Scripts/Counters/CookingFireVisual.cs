using System;
using UnityEngine;

public class CookingFireVisual : MonoBehaviour
{
    [SerializeField] private CookingFireCounter cookingFire;
    [SerializeField] private GameObject particles;

    private void Start()
    {
        cookingFire.OnStateChanged += UpdateVisual;
    }

    private void UpdateVisual(object sender, CookingFireCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == CookingFireCounter.State.Cooking || e.state == CookingFireCounter.State.Cooked;
        particles.SetActive(showVisual);
    }
}

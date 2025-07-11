using System;
using UnityEngine;

public class RecipeShelf : BaseCounter
{
    public event EventHandler OnPlayerSelectedShelf;

    public override void Interact(PlayerController player)
    {
        OnPlayerSelectedShelf?.Invoke(this, EventArgs.Empty);
        player.enabled = false;
    }
}

//------------------------------------------------------------------------------
// Container Counter Script:
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField]
    private KitchenObjectSO kitchenObjectSO; // type of kitchen object that can be spawned on this counter.

    public override void Interact(Player player) // override Interact() method from BaseCounter.
    {
        if (!player.HasKitchenObject()) // player is not carrying anything
        {
            // spawn kitchenObject and give it to the player, then trigger grab event.
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}

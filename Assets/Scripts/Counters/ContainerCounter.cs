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

    // type of kitchen object that can be spawned on this counter.
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // override Interact() method from BaseCounter.
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) // player is not carrying anything
        {
            // spawn kitchenObject and give it to the player, then trigger grab event.
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}

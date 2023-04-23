//------------------------------------------------------------------------------
// Clear Counter Script:
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    // TODO: Do I need this kitchenObjectSO field since Clear Counters will only be getting/returning kitchen objects from the player?
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) // override Interact() method from BaseCounter.
    {
        if (!HasKitchenObject()) // the counter is empty.
        {
            if (player.HasKitchenObject()) // the player is carrying a kitchenObject
            {
                // transfer kitchenObject from the player to the counter.
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // the player is not carrying anything.
            }
        }
        else // the counter has a kitchen object.
        {
            if (player.HasKitchenObject()) // the player is carrying a kitchen object
            {
                // the player cant pick up another object.
            }
            else
            {
                // the player picks up the kitchen object from the counter.
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}

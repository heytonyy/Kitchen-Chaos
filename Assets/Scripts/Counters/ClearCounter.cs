//------------------------------------------------------------------------------
// Clear Counter Script:
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // override Interact() method from BaseCounter.
    public override void Interact(Player player)
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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) // the player is carrying a plate.
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else // player is carrying a kitchen object that is not a plate, try to add it to plate.
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) // counter has a plate.
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // the player picks up the kitchen object from the counter.
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}

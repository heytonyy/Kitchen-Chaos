//------------------------------------------------------------------------------
// Cutting Counter Script:
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IHasProgress
{
    // event listeners when the progress of the counters changes.
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut; // event listener when player cuts the kitchen object.

    [SerializeField]
    private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) // the counter is empty.
        {
            if (player.HasKitchenObject()) // the player is carrying a kitchenObject
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) // kitchen object can be cut on counter.
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    // update the progress bar.
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(
                        GetKitchenObject().GetKitchenObjectSO()
                    );
                    OnProgressChanged?.Invoke(
                        this,
                        new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized =
                                (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                        }
                    );
                }
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
            }
            else // the player is not carying anything
            {
                // pick up the kitchen object from the counter.
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // there is a kitchen object on the counter that can be cut.
            // increase the cutting progress.
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            // get the cutting recipe for the input kitchen object.
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(
                this,
                new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                }
            );

            // check if the cutting progress is complete.
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                // output is the slices of the input kitchen object.
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                // destroy it and spawn the output.
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}

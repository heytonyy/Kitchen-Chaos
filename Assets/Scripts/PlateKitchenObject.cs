//------------------------------------------------------------------------------
// Plate Kitchen Object Script:
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<IngredientAddedEventArgs> OnIngredientAdded;
    public class IngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) // invalid ingredient on plate
        {
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) // ingredient already on plate
        {
            return false;
        }
        else // add ingredient to plate
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new IngredientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}

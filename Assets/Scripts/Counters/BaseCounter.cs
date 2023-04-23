//------------------------------------------------------------------------------
// Base Counter Script:
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    private KitchenObject kitchenObject;

    // can use either Transform or GameObject.
    [SerializeField]
    private Transform counterTopPoint; // reference point for instantiating kitchenObject.

    public virtual void Interact(Player player) // Interact/InteractAlternate methods are overridden in child classes.
    {
        Debug.LogError("BaseCounter.Interact() called!");
    }

    public virtual void InteractAlternate(Player player)
    {
        // Debug.LogError("BaseCounter.InteractAlternate() called!");
    }

    // IKitchenObjectParent interface methods.
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}

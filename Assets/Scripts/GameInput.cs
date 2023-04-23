//------------------------------------------------------------------------------
// Game Input Script:
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        // New Input System: create new PlayerInputActions object and enable it.
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        // Subscribe to the Interact action (pick up/drop).
        playerInputActions.Player.Interact.performed += Interact_performed;
        // Subscribe to the InteractAlternate action (cut).
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    // pick up/drop action.
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // cut action.
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}

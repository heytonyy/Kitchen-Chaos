//------------------------------------------------------------------------------
// Player Class Script:
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    // extend EventArgs to pass selectedCounter data -> OnSelectedCounterChanged event.
    public event EventHandler<SelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class SelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    // Singleton pattern (using since only 1 player in scene) -> instance property can use { get; set; } shorthand.
    public static Player Instance { get; private set; }
    // player input reference.
    [SerializeField] private GameInput gameInput;

    // KitchenObjectHoldPoint -> Transform for holding kitchenObject.
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;

    // BaseCounter -> Counter that player is currently interacting with.
    private BaseCounter selectedCounter;
    // counter layer mask -> for raycasting groups of counters.
    [SerializeField] private LayerMask counterLayerMask;

    [SerializeField] private float moveSpeed = 7f;
    private bool isWalking;
    private Vector3 lastInteractDirection;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Player already exists!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    // Subscribe to the OnInteractAction event.
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    // Subscribe to the OnInteractAlternateAction event.
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        // TODO: refactor so OnSelectedCounterChanged is not called if this.selectedCounter is null and selectedCounter is null.
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(
            this,
            new SelectedCounterChangedEventArgs { selectedCounter = selectedCounter }
        );
    }

    private void HandleInteractions()
    {
        // Get input vector and convert to moveDirection
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        // If player is moving, set lastInteractDirection to moveDirection.
        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }

        // Raycast to check if there is a ClearCounter in front of the player.
        float interactDistance = 2f;
        // Raycast hit something
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            // Hit a BaseCounter (raycastHit is info about the object that was hit, out is the returned BaseCounter)
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // If the BaseCounter is not the same as the selectedCounter, set it as the selectedCounter.
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            // Hit something else
            else
            {
                SetSelectedCounter(null);
            }
        }
        // Raycast hit nothing
        else
        {
            SetSelectedCounter(null);
        }
        // Debug.Log("Selected counter: " + selectedCounter);
    }

    private void HandleMovement()
    {
        // Get input vector
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        // determine if player can move in moveDirection
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        float moveDistance = Time.deltaTime * moveSpeed;
        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDirection,
            moveDistance
        );

        // Cannot move towards moveDirection
        if (!canMove)
        {
            // Attempt only X movement
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0F, 0f).normalized;
            // moveDirection.x != 0 -> to make player still rotate when moving diagonally
            canMove = moveDirection.x != 0
                && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            if (canMove)
            {
                // Can move only on the X
                moveDirection = moveDirectionX;
            }
            else
            {
                // Cannot move on the X
                // Attempt only Z movement
                Vector3 moveDirectionZ = new Vector3(0f, 0F, moveDirection.z);
                // moveDirection.z != 0 -> to make player still rotate when moving diagonally
                canMove = moveDirection.z != 0
                    && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                if (canMove)
                {
                    // Can move only on the Z
                    moveDirection = moveDirectionZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        // translate player position
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        // set isWalking to true if player is moving
        isWalking = moveDirection != Vector3.zero;

        // rotate player to face moveDirection, slerp for smooth rotation (lerp -> linear, slerp -> smooth)
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    // for walking animation in PlayerAnimator.cs
    public bool IsWalking()
    {
        return isWalking;
    }

    // IKitchenObjectParent interface methods.
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
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

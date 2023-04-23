//------------------------------------------------------------------------------
// Player Animator Script:
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // from animator parameters.
    private const string IS_WALKING = "IsWalking";

    [SerializeField]
    private Player player;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    // changes the value of the IsWalking parameter in the animator if the player is walking.
    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}

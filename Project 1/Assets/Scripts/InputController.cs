using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    //feeds inputs to movement conntroler
    [SerializeField]
    MovementController movementController;
    [SerializeField]
    PlayerParticles playerParticles;
    [SerializeField]
    PlayerInfo playerInfo;

    private Vector2 directionBeforeZero = Vector2.zero;

    //change the players direction and activate particles
    public void OnMove(InputAction.CallbackContext context)
    {
        movementController.SetDirection(context.ReadValue<Vector2>());
        //only update direction if not zeroing out
        if (!movementController.ZeroOut)
        {
            playerParticles.OnMove(context.ReadValue<Vector2>());
        }
        //keeps track of direction if particles need to be updated when zero out ends
        directionBeforeZero = context.ReadValue<Vector2>();
    }

    //toggles between the player shooting and not shooting
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInfo.IsShooting = true;
        }
        else if (context.canceled)
        {
            playerInfo.IsShooting = false;
        }
    }

    //attempts to boost the player and change the particles
    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed && !movementController.ZeroOut)
        {
            movementController.Boost();
        }    
    }

    //when held, try to set the player velocity to 0
    public void OnZeroOut(InputAction.CallbackContext context)
    {
        //pressing space zeros out and deactivates particles
        if (context.performed)
        {
            movementController.ZeroOut = true;
            playerParticles.OnMove(Vector2.zero);
            playerParticles.OnZero(movementController.Velocity);
        }
        //releasing space disables zero out and updates direction for particles
        if (context.canceled)
        {
            movementController.ZeroOut = false;
            playerParticles.OnMove(directionBeforeZero);
            playerParticles.OnZero(Vector3.zero);
        }
    }
}

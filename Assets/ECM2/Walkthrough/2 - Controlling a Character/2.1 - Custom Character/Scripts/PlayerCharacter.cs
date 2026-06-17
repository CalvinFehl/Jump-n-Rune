using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM2.Walkthrough.Ex21
{
    /// <summary>
    /// This example shows how to extend a Character (through inheritance)
    /// adding input management.
    /// </summary>
    
    public class PlayerCharacter : Character
    {
        private void Update()
        {
            // Movement input
            Vector2 inputMove = Keyboard.current != null ? new Vector2()
            {
                x = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
                y = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
            } : Vector2.zero;
            
            Vector3 movementDirection = Vector3.zero;

            movementDirection += Vector3.right * inputMove.x;
            movementDirection += Vector3.forward * inputMove.y;
            
            // If character has a camera assigned...
            
            if (camera)
            {
                // Make movement direction relative to its camera view direction
                
                movementDirection = movementDirection.relativeTo(cameraTransform);
            }

            SetMovementDirection(movementDirection);
            
            // Crouch input
            
            if (Keyboard.current != null && (Keyboard.current.leftCtrlKey.wasPressedThisFrame || Keyboard.current.cKey.wasPressedThisFrame))
                Crouch();
            else if (Keyboard.current != null && (Keyboard.current.leftCtrlKey.wasReleasedThisFrame || Keyboard.current.cKey.wasReleasedThisFrame))
                UnCrouch();
            
            // Jump input
            
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                Jump();
            else if (Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame)
                StopJumping();
        }
    }
}
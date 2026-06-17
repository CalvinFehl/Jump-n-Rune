using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM2.Walkthrough.Ex51
{
    /// <summary>
    /// This example shows how to handle Character events when extending a Character through inheritance.
    /// </summary>
    
    public class PlayerCharacter : Character
    {
        protected override void OnCollided(ref CollisionResult collisionResult)
        {
            // Call base method implementation
            
            base.OnCollided(ref collisionResult);
            
            // Add your code here...
            
            Debug.Log($"Collided with {collisionResult.collider.name}");
        }

        protected override void OnFoundGround(ref FindGroundResult foundGround)
        {
            // Call base method implementation
            
            base.OnFoundGround(ref foundGround);
            
            // Add your code here...
            
            Debug.Log($"Found {foundGround.collider.name} ground");
        }

        protected override void OnLanded(Vector3 landingVelocity)
        {
            // Call base method implementation
            
            base.OnLanded(landingVelocity);
            
            // Add your code here...
            
            Debug.Log($"Landed with {landingVelocity:F4} landing velocity.");
        }

        protected override void OnCrouched()
        {
            // Call base method implementation
            
            base.OnCrouched();
            
            // Add your code here...
            
            Debug.Log("Crouched");
        }

        protected override void OnUnCrouched()
        {
            // Call base method implementation
            
            base.OnUnCrouched();
            
            // Add your code here...
            
            Debug.Log("UnCrouched");
        }

        protected override void OnJumped()
        {
            // Call base method implementation
            
            base.OnJumped();
            
            // Add your code here...
            
            Debug.Log("Jumped!");
            
            // Enable apex notification event
            
            notifyJumpApex = true;
        }

        protected override void OnReachedJumpApex()
        {
            // Call base method implementation
            
            base.OnReachedJumpApex();
            
            // Add your code here...
            
            Debug.Log($"Apex reached {GetVelocity():F4}");
        }

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
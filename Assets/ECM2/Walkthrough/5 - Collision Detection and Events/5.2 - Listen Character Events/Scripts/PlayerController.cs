using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM2.Walkthrough.Ex52
{
    /// <summary>
    /// This example shows how to listen to Character events when extending a Character through composition.
    /// </summary>
    
    public class PlayerController : MonoBehaviour
    {
        // The controlled Character
        
        private Character _character;
        
        protected void OnCollided(ref CollisionResult collisionResult)
        {
            Debug.Log($"Collided with {collisionResult.collider.name}");
        }

        protected void OnFoundGround(ref FindGroundResult foundGround)
        {
            Debug.Log($"Found {foundGround.collider.name} ground");
        }

        protected void OnLanded(Vector3 landingVelocity)
        {
            Debug.Log($"Landed with {landingVelocity:F4} landing velocity.");
        }

        protected void OnCrouched()
        {
            Debug.Log("Crouched");
        }

        protected void OnUnCrouched()
        {
            Debug.Log("UnCrouched");
        }

        protected void OnJumped()
        {
            Debug.Log("Jumped!");
            
            // Enable apex notification event
            
            _character.notifyJumpApex = true;
        }

        protected void OnReachedJumpApex()
        {
            Debug.Log($"Apex reached {_character.GetVelocity():F4}");
        }

        private void Awake()
        {
            // Cache controlled character
            
            _character = GetComponent<Character>();
        }

        private void OnEnable()
        {
            // Subscribe to Character events
            
            _character.Collided += OnCollided;
            _character.FoundGround += OnFoundGround;
            _character.Landed += OnLanded;
            _character.Crouched += OnCrouched;
            _character.UnCrouched += OnUnCrouched;
            _character.Jumped += OnJumped;
            _character.ReachedJumpApex += OnReachedJumpApex;
        }

        private void OnDisable()
        {
            // Un-subscribe from Character events
            
            _character.Collided -= OnCollided;
            _character.FoundGround -= OnFoundGround;
            _character.Landed -= OnLanded;
            _character.Crouched -= OnCrouched;
            _character.UnCrouched -= OnUnCrouched;
            _character.Jumped -= OnJumped;
            _character.ReachedJumpApex -= OnReachedJumpApex;
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
            
            if (_character.camera)
            {
                // Make movement direction relative to its camera view direction
                
                movementDirection = movementDirection.relativeTo(_character.cameraTransform);
            }

            _character.SetMovementDirection(movementDirection);
            
            // Crouch input
            
            if (Keyboard.current != null && (Keyboard.current.leftCtrlKey.wasPressedThisFrame || Keyboard.current.cKey.wasPressedThisFrame))
                _character.Crouch();
            else if (Keyboard.current != null && (Keyboard.current.leftCtrlKey.wasReleasedThisFrame || Keyboard.current.cKey.wasReleasedThisFrame))
                _character.UnCrouch();
            
            // Jump input
            
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                _character.Jump();
            else if (Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame)
                _character.StopJumping();
        }
    }
}
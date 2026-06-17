using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM2.Examples.Glide
{
    public class PlayerController : MonoBehaviour
    {
        private Character _character;
        private GlideAbility _glideAbility;

        private void Awake()
        {
            _character = GetComponent<Character>();
            _glideAbility = GetComponent<GlideAbility>();
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
            
            // Glide input
            
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                _glideAbility.Glide();
            else if (Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame)
                _glideAbility.StopGliding();
        }
    }
}
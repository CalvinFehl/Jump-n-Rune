using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM2.Examples.FirstPersonFly
{
    /// <summary>
    /// Regular First Person Character Input. Shows how to handle movement while flying.
    /// In this case, we allow to fly towards our view direction, allowing to freely move through the air. 
    /// </summary>
    
    public class CharacterInput : MonoBehaviour
    {
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            Vector2 inputMove = Keyboard.current != null ? new Vector2()
            {
                x = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
                y = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
            } : Vector2.zero;
            
            Vector3 movementDirection = Vector3.zero;

            if (_character.IsFlying())
            {
                // Strafe
                
                movementDirection += _character.GetRightVector() * inputMove.x;
                
                // Forward, along camera view direction (if any) or along character's forward if camera not found 

                Vector3 forward =
                    _character.camera ? _character.cameraTransform.forward : _character.GetForwardVector();

                movementDirection += forward * inputMove.y;
                
                // Vertical movement

                if (_character.jumpInputPressed)
                    movementDirection += Vector3.up;
            }
            else
            {
                // Regular First Person movement relative to character's view direction
                
                movementDirection += _character.GetRightVector() * inputMove.x;
                movementDirection += _character.GetForwardVector() * inputMove.y;
            }

            _character.SetMovementDirection(movementDirection);
            
            if (Keyboard.current != null && (Keyboard.current.leftCtrlKey.wasPressedThisFrame || Keyboard.current.cKey.wasPressedThisFrame))
                _character.Crouch();
            else if (Keyboard.current != null && (Keyboard.current.leftCtrlKey.wasReleasedThisFrame || Keyboard.current.cKey.wasReleasedThisFrame))
                _character.UnCrouch();
            
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                _character.Jump();
            else if (Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame)
                _character.StopJumping();
        }
    }
}

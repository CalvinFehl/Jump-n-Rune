using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM2.Examples.FirstPerson
{
    /// <summary>
    /// First person character input.
    /// </summary>
    
    public class FirstPersonCharacterInput : MonoBehaviour
    {
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            // Movement input, relative to character's view direction
            Vector2 inputMove = Keyboard.current != null ? new Vector2()
            {
                x = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
                y = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
            } : Vector2.zero;
            
            Vector3 movementDirection = Vector3.zero;
            
            movementDirection += _character.GetRightVector() * inputMove.x;
            movementDirection += _character.GetForwardVector() * inputMove.y;

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

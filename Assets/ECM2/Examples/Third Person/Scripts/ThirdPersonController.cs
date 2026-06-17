using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM2.Examples.ThirdPerson
{
    /// <summary>
    /// This example shows how to implement a basic third person controller.
    /// This must be added to a Character.
    /// </summary>
    
    public class ThirdPersonController : MonoBehaviour
    {
        [Space(15.0f)]
        public GameObject followTarget;

        [Tooltip("The default distance behind the Follow target.")]
        [SerializeField]
        public float followDistance = 5.0f;

        [Tooltip("The minimum distance to Follow target.")]
        [SerializeField]
        public float followMinDistance;

        [Tooltip("The maximum distance to Follow target.")]
        [SerializeField]
        public float followMaxDistance = 10.0f;

        [Space(15.0f)]
        public bool invertLook = true;

        [Tooltip("Mouse look sensitivity")]
        public Vector2 mouseSensitivity = new Vector2(1.0f, 1.0f);

        [Space(15.0f)]
        [Tooltip("How far in degrees can you move the camera down.")]
        public float minPitch = -80.0f;

        [Tooltip("How far in degrees can you move the camera up.")]
        public float maxPitch = 80.0f;

        protected float _cameraYaw;
        protected float _cameraPitch;

        protected float _currentFollowDistance;
        protected float _followDistanceSmoothVelocity;

        protected Character _character;
        
        /// <summary>
        /// Add input (affecting Yaw).
        /// This is applied to the camera's rotation.
        /// </summary>

        public virtual void AddControlYawInput(float value)
        {
            _cameraYaw = MathLib.ClampAngle(_cameraYaw + value, -180.0f, 180.0f);
        }
        
        /// <summary>
        /// Add input (affecting Pitch).
        /// This is applied to the camera's rotation.
        /// </summary>

        public virtual void AddControlPitchInput(float value, float minValue = -80.0f, float maxValue = 80.0f)
        {
            _cameraPitch = MathLib.ClampAngle(_cameraPitch + value, minValue, maxValue);
        }
        
        /// <summary>
        /// Adds input (affecting follow distance).
        /// </summary>

        public virtual void AddControlZoomInput(float value)
        {
            followDistance = Mathf.Clamp(followDistance - value, followMinDistance, followMaxDistance);
        }
        
        /// <summary>
        /// Update camera's rotation applying current _cameraPitch and _cameraYaw values.
        /// </summary>

        protected virtual void UpdateCameraRotation()
        {
            Transform cameraTransform = _character.cameraTransform;
            cameraTransform.rotation = Quaternion.Euler(_cameraPitch, _cameraYaw, 0.0f);
        }
        
        /// <summary>
        /// Update camera's position maintaining _currentFollowDistance from target. 
        /// </summary>

        protected virtual void UpdateCameraPosition()
        {
            Transform cameraTransform = _character.cameraTransform;
            
            _currentFollowDistance =
                Mathf.SmoothDamp(_currentFollowDistance, followDistance, ref _followDistanceSmoothVelocity, 0.1f);

            cameraTransform.position =
                followTarget.transform.position - cameraTransform.forward * _currentFollowDistance;
        }
        
        /// <summary>
        /// Update camera's position and rotation.
        /// </summary>

        protected virtual void UpdateCamera()
        {
            UpdateCameraRotation();
            UpdateCameraPosition();
        }

        protected virtual void Awake()
        {
            _character = GetComponent<Character>();
        }

        protected virtual void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            Vector3 euler = _character.cameraTransform.eulerAngles;

            _cameraPitch = euler.x;
            _cameraYaw = euler.y;

            _currentFollowDistance = followDistance;
        }

        protected virtual void Update()
        {
            // Movement input
            Vector2 inputMove = Keyboard.current != null ? new Vector2
            {
                x = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
                y = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
            } : Vector2.zero;

            Vector3 movementDirection = Vector3.zero;

            movementDirection += Vector3.right * inputMove.x;
            movementDirection += Vector3.forward * inputMove.y;

            if (_character.cameraTransform)
                movementDirection = movementDirection.relativeTo(_character.cameraTransform, _character.GetUpVector());

            _character.SetMovementDirection(movementDirection);
            
            // Crouch input
            if ((Keyboard.current != null && (Keyboard.current.leftCtrlKey.wasPressedThisFrame || Keyboard.current.cKey.wasPressedThisFrame)))
                _character.Crouch();
            else if (Keyboard.current != null && (Keyboard.current.leftCtrlKey.wasReleasedThisFrame || Keyboard.current.cKey.wasReleasedThisFrame))
                _character.UnCrouch();
            
            // Jump input
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                _character.Jump();
            else if (Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame)
                _character.StopJumping();
            
            // Look input
            Vector2 lookInput = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
            lookInput *= mouseSensitivity;

            AddControlYawInput(lookInput.x);
            AddControlPitchInput(invertLook ? -lookInput.y : lookInput.y, minPitch, maxPitch);
            
            // Zoom input
            float mouseScrollInput = Mouse.current != null ? Mouse.current.scroll.y.ReadValue() : 0f;
            AddControlZoomInput(mouseScrollInput);
        }

        protected virtual void LateUpdate()
        {
            UpdateCamera();
        }
    }
}

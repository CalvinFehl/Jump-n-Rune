using UnityEngine;
using UnityEngine.InputSystem;

public class FollowLaserPointerOnClick : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LaserPointer laserPointer;
    [SerializeField] private Transform target;
    public Vector3 targetPosition;

    [Header("Settings")]
    [SerializeField] private float wallOffset = 0.2f, followSpeed = 5f, distanceThreshold = 0.1f;

    private bool isFollowing = false;

    private void Update()
    {
        if (laserPointer == null || target == null)
        {
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isFollowing = !isFollowing;
            target.position = laserPointer.laserHitPos;
            targetPosition = target.position + laserPointer.laserHitNormal * wallOffset;
        }

        if (isFollowing)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < distanceThreshold)
            {
                isFollowing = false;
            }
        }
    }
}
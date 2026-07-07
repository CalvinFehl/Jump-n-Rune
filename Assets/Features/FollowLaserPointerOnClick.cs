using UnityEngine;
using UnityEngine.InputSystem;

public class FollowLaserPointerOnClick : MonoBehaviour
{
    [SerializeField] private LaserPointer laserPointer;
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f, distanceThreshold = 0.1f;

    private bool isFollowing = false;

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isFollowing = !isFollowing;
            target.position = laserPointer.laserHitPos;
        }

        if (isFollowing && laserPointer != null && target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) < distanceThreshold)
            {
                isFollowing = false;
            }
        }
    }
}
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
            transform.parent = null;
            Debug.Log("You got Batman'd! No Parents");
            isFollowing = !isFollowing;

            target.position = laserPointer.LaserHitPos;
            targetPosition = target.position + laserPointer.LaserHitNormal * wallOffset;
        }

        if (isFollowing)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < distanceThreshold)
            {
                isFollowing = false;

                if (laserPointer.LaserHitObject != null && laserPointer.LaserHitObject.CompareTag("Sticky"))
                {
                    transform.parent = laserPointer.LaserHitObject.transform;
                    Debug.Log("Entered Sticky");
                }
            }
        }
    }
}
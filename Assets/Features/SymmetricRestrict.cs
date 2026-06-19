using Unity.VisualScripting;
using UnityEngine;

public class SymmetricRestrict : MonoBehaviour
{
    [SerializeField] private SymmetricRestrict restrictionBuddy;
    [SerializeField] private Collider xPos, xNeg, yPos, yNeg, zPos, zNeg;
    public bool MoveX, MoveXNeg, MoveY, MoveYNeg, MoveZ, MoveZNeg;

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.activeSelf)
        {
            if (collision == xPos)
            {
                MoveX = false;
                restrictionBuddy.MoveXNeg = false;
            }
            else if (collision == xNeg)
            {
                MoveXNeg = false;
                restrictionBuddy.MoveX = false;
            }
            else if (collision == yPos)
            {
                MoveY = false;
                restrictionBuddy.MoveYNeg = false;
            }
            else if (collision == yNeg)
            {
                MoveYNeg = false;
                restrictionBuddy.MoveY = false;
            }
            else if (collision == zPos)
            {
                MoveZ = false;
                restrictionBuddy.MoveZNeg = false;
            }
            else if (collision == zNeg)
            {
                MoveZNeg = false;
                restrictionBuddy.MoveZ = false;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.activeSelf)
        {
            if (collision == xPos)
            {
                MoveX = true;
                restrictionBuddy.MoveXNeg = true;
            }
            else if (collision == xNeg)
            {
                MoveXNeg = true;
                restrictionBuddy.MoveX = true;
            }
            else if (collision == yPos)
            {
                MoveY = true;
                restrictionBuddy.MoveYNeg = true;
            }
            else if (collision == yNeg)
            {
                MoveYNeg = true;
                restrictionBuddy.MoveY = true;
            }
            else if (collision == zPos)
            {
                MoveZ = true;
                restrictionBuddy.MoveZNeg = true;
            }
            else if (collision == zNeg)
            {
                MoveZNeg = true;
                restrictionBuddy.MoveZ = true;
            }
        }
    }

    private void LateUpdate()
    {
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = new Vector3(
                (!MoveX && rb.linearVelocity.x > 0) || (!MoveXNeg && rb.linearVelocity.x < 0) ? 0 : rb.linearVelocity.x,
                (!MoveY && rb.linearVelocity.y > 0) || (!MoveYNeg && rb.linearVelocity.y < 0) ? 0 : rb.linearVelocity.y,
                (!MoveZ && rb.linearVelocity.z > 0) || (!MoveZNeg && rb.linearVelocity.z < 0) ? 0 : rb.linearVelocity.z
            );
        }
    }
}

using UnityEngine;
using System.Collections;

public class LeafTipBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody leafTipRB;
    [SerializeField] private Transform MoveGoalTransform;
    [SerializeField] public Vector3 MoveForce;
    [SerializeField] public float MoveForceMagnitude = 120;
    [SerializeField] private float massChangeDuration;
    [SerializeField] public bool ForceIsActive;

    void Start()
    {
        if (leafTipRB == null)
        {
            leafTipRB = GetComponent<Rigidbody>();
        }
    }

    public void ToggleMoveForce(bool activate, Transform moveGoalTransform = null)
    {
        ForceIsActive = activate;
        if (moveGoalTransform != null)
        {
            MoveGoalTransform = moveGoalTransform;
        }
    }

    void FixedUpdate()
    {
        if (ForceIsActive && leafTipRB != null && MoveGoalTransform != null)
        {
            Vector3 moveDirection = (MoveGoalTransform.position - transform.position).normalized;
            leafTipRB.AddForce(moveDirection * MoveForceMagnitude);
        }
    }
}
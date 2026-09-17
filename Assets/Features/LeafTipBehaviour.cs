using UnityEngine;

public class LeafTipBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody leafTipRB;
    [SerializeField] private Transform MoveGoalTransform;

    [Header("Force Settings")]
    [SerializeField] private float maxAffectedDistance = 10f;
    [Tooltip("If true, the leaf tip will rotate to face the MoveGoalTransform while moving towards it.")]
    [SerializeField] private bool turnsTowardsGoal = true;
    [Tooltip("If greater than 0, the leaf tip will check for line of sight to the MoveGoalTransform in an intervall before applying force.")]
    [SerializeField] private float sightlineChecks = -1f;

    [SerializeField] public float MoveForceMagnitude = 120;
    [SerializeField] public float TurnForceMagnitude = 120;

    [Header("Runtime Variables")]
    [SerializeField] public Vector3 MoveForce;
    [SerializeField] public bool ForceIsActive;
    private float lastSightlineCheckTime;

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
        if (sightlineChecks > 0f)
        {
            if (lastSightlineCheckTime + sightlineChecks < Time.time)
            {
                lastSightlineCheckTime = Time.time;
                ForceIsActive = CheckLineOfSight();
            }
        }

        if (ForceIsActive && leafTipRB != null && MoveGoalTransform != null)
        {
            Vector3 moveDirection = (MoveGoalTransform.position - transform.position).normalized;
            leafTipRB.AddForce(moveDirection * MoveForceMagnitude);

            if (turnsTowardsGoal)
            {
                //Hier Code aus Hoverboard einfügen
            }
        }
    }

    private bool CheckLineOfSight()
    {
        if (MoveGoalTransform == null)
        {
            return false;
        }

        if (Vector3.Distance(transform.position, MoveGoalTransform.position) > maxAffectedDistance)
        {
            return false;
        }

        Physics.Raycast(MoveGoalTransform.position, (transform.position - MoveGoalTransform.position).normalized, out RaycastHit hitInfo);
        if (hitInfo.collider != null && hitInfo.collider.transform != this.transform)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
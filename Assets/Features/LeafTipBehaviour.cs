using System.Collections.Generic;
using UnityEngine;

public class LeafTipBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody leafTipRB;
    [SerializeField] private Transform MoveGoalTransform;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("LineRendererSettings")]
    [Tooltip("Update each X-th frame")]
    [SerializeField] private int updateRate = 3;
    [SerializeField] private List<Transform> segments;

    [Header("Force Settings")]
    [SerializeField] private float maxAffectedDistance = 10f;
    [Tooltip("If true, the leaf tip will rotate to face the MoveGoalTransform while moving towards it.")]
    [SerializeField] private bool turnsTowardsGoal = true;
    [Tooltip("If greater than 0, the leaf tip will check for line of sight to the MoveGoalTransform in an intervall before applying force.")]
    [SerializeField] private float sightlineChecks = -1f;
    [SerializeField] private float reactionSpeed = 0.02f;
    [SerializeField] private float forceTreshold = 0.1f;
    // forceEaseInDuration = 0.5f, forceEaseOutDuration = 0f;

    [SerializeField] public float MoveForceMagnitude = 120;
    [SerializeField] public float TurnForceMagnitude = 120;

    [Header("Runtime Variables")]
    [SerializeField] public Vector3 MoveForce;
    [SerializeField] public bool ForceIsActive, WillCheckLineOfSight;
    // [SerializeField] private float forceEaseMultiplier;
    private float lastSightlineCheckTime;
    private Vector3 lastMoveForce;
    private int frameCounter = 0;

    void Start()
    {
        if (leafTipRB == null)
        {
            leafTipRB = GetComponent<Rigidbody>();
        }

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();

          
        }
        
        if (lineRenderer?.positionCount != segments.Count) 
        {
            lineRenderer.positionCount = segments.Count;
        }  

        frameCounter = updateRate;
        CalculateLineRenderer();
    }

    public void ToggleMoveForce(bool activate, Transform moveGoalTransform = null)
    {
        WillCheckLineOfSight = activate;
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
                if (WillCheckLineOfSight) ForceIsActive = CheckLineOfSight();
            }
        }

        if (leafTipRB != null && MoveGoalTransform != null)
        {
            Vector3 thisFrameForce = ForceIsActive ? (MoveGoalTransform.position - transform.position).normalized * MoveForceMagnitude : Vector3.zero;

            Vector3 newForce = lastMoveForce * (1 - reactionSpeed * Time.deltaTime) + thisFrameForce * reactionSpeed * Time.deltaTime ;
            
            lastMoveForce = newForce;

            if (lastMoveForce.magnitude > forceTreshold)
            {
                leafTipRB.AddForce(newForce);                
            }

            if (turnsTowardsGoal)
            {
                //Hier Code aus Hoverboard einfügen
            }
        }

        CalculateLineRenderer();
    }

    private void CalculateLineRenderer()
    {
        if (lineRenderer != null && segments.Count > 1)
        {
            frameCounter++;
            if (frameCounter >= updateRate)
            {
                frameCounter = 0;
                for (int i = 0; i < segments.Count; i++)
                {
                    lineRenderer.SetPosition(i, segments[i].position);
                }
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

        Physics.Raycast(MoveGoalTransform.position, (transform.position - MoveGoalTransform.position).normalized, out RaycastHit hitInfo, maxAffectedDistance, ~LayerMask.GetMask("Plant"));

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
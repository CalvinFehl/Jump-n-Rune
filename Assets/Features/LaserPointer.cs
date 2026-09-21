using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform, rayOrigin, laserOrigin;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Settings")]
    [SerializeField] private float maxDistance = 30f;
    [SerializeField] private bool laserActive = true;
    
    [Header("Public Runtime Variables")]
    public Vector3 LaserHitPos, LaserHitNormal;
    public GameObject LaserHitObject;


    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (laserOrigin == null)
        {
            laserOrigin = transform;
        }

        if (rayOrigin == null)
        {
            rayOrigin = laserOrigin ?? transform;
        }

    }

    void LateUpdate()
    {
        if (laserActive && cameraTransform != null)
        {
            lineRenderer.enabled = true;
            Ray ray = new Ray(rayOrigin.position, cameraTransform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                lineRenderer.SetPosition(0, laserOrigin.position);
                lineRenderer.SetPosition(1, hit.point);
                LaserHitPos = hit.point;
                LaserHitNormal = hit.normal;
                LaserHitObject = hit.collider.gameObject;
            }
            else
            {
                lineRenderer.SetPosition(0, laserOrigin.position);
                lineRenderer.SetPosition(1, ray.origin + ray.direction * maxDistance);
                LaserHitPos = ray.origin + ray.direction * maxDistance;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}

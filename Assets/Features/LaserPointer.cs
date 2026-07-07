using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform, rayOrigin, laserOrigin;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxDistance = 30f;
    [SerializeField] private bool laserActive = true;
    [SerializeField] public Vector3 laserHitPos;

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
                laserHitPos = hit.point;
            }
            else
            {
                lineRenderer.SetPosition(0, laserOrigin.position);
                lineRenderer.SetPosition(1, ray.origin + ray.direction * maxDistance);
                laserHitPos = ray.origin + ray.direction * maxDistance;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}

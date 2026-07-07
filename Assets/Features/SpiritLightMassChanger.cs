using UnityEngine;

public class SpiritLightMassChanger : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider;

    void Awake()
    {
        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider>();
        }
    }

    void Start()
    {
        CheckNearbyLeafTip();
    }

    private void OnEnable()
    {
        CheckNearbyLeafTip();
    }

    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<LeafTipBehaviour>()?.ToggleMoveForce(true, this.transform);
    }

    void OnTriggerExit(Collider other)
    {
        other.GetComponent<LeafTipBehaviour>()?.ToggleMoveForce(false);
    }

    public void CheckNearbyLeafTip()
    {
        if (triggerCollider == null)
        {
            return;
        }

        var bounds = triggerCollider.bounds;
        var colliders = Physics.OverlapBox(bounds.center, bounds.extents, triggerCollider.transform.rotation);

        foreach (var hit in colliders)
        {
            hit.GetComponent<LeafTipBehaviour>()?.ToggleMoveForce(true);
        }
    }
}

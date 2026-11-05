using UnityEngine;

public class BendTowards : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float bendStrength = 1.0f;
    [SerializeField] private float influenceRadius = 3.0f;
    [SerializeField] private Material material;



    void Update()
    {
        if (target != null && material != null)
        {
            Vector3 lightPosition = target.position;
            material.SetVector("_LightPosition", new Vector4(lightPosition.x, lightPosition.y, lightPosition.z, 1.0f));
            material.SetFloat("_LightInfluence", bendStrength);
            material.SetFloat("_InfluenceRadius", influenceRadius);
        }
    }
}

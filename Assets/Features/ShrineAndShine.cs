using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineAndShine : MonoBehaviour
{
    [SerializeField] private List<Light> lights = new();
    private List<float> lightIntensities = new();

    [SerializeField] private float lightIncreaseSpeed = 1f;
    [SerializeField] private bool isActivated = false;

    void Awake()
    {
        // Ensure all lights are off at the start
        foreach (var light in lights)
        {
            lightIntensities.Add(light.intensity);
            light.enabled = false;
            light.intensity = 0;
        }

        isActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("SpiritLight"))
        {
            isActivated = true;
            ActivateShrine();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isActivated && other.CompareTag("SpiritLight"))
        {
            isActivated = false;
            DeactivateShrine();
        }
    }

    public void ActivateShrine()
    {
        StartCoroutine(IncreaseLightIntensity());
    }

    public void DeactivateShrine()
    {
        StartCoroutine(IncreaseLightIntensity(false));
    }

    IEnumerator IncreaseLightIntensity(bool activate = true)
    {
        float elapsedTime = 0f;
        while (elapsedTime < lightIncreaseSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lightIncreaseSpeed);
            for (int i = 0; i < lights.Count; i++)
            {
                if (activate)
                {
                    lights[i].intensity = Mathf.Lerp(0, lightIntensities[i], t);
                }
                else
                {
                    lights[i].intensity = Mathf.Lerp(lightIntensities[i], 0, t);
                }
            }
            yield return null;
        }

        for (int i = 0; i < lights.Count; i++)
        {
            if (activate)
            {
                lights[i].enabled = true;
                lights[i].intensity = lightIntensities[i];
            }
            else
            {
                lights[i].enabled = false;
                lights[i].intensity = 0;
            }
        }
    }
}

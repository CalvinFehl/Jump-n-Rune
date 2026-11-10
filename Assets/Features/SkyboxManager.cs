using UnityEngine;
using System.Collections;
using UnityEditor;

public class SkyboxManager : MonoBehaviour
{
    private Material currentSkybox;
    [SerializeField] private Color nightColor;
    [SerializeField] private float nightExposure = 0.56f;
    [SerializeField] private Color dawnColor;
    [SerializeField] private float dawnExposure = 1.12f;
    [SerializeField] private float transitionDuration = 5f;
    [SerializeField] private GameObject targetObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSkybox = RenderSettings.skybox;
        if(currentSkybox == null)
        {
            Debug.LogError("No skybox material found in RenderSettings.");
            return;
        }
        currentSkybox.SetColor("_TintColor", nightColor);
        currentSkybox.SetFloat("_Exposure", nightExposure);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if (other.GetComponent<TriggerLightTag>() != null)
        {
            Debug.Log("TriggerLightTag detected. Starting skybox color change.");
            StartCoroutine(ChangeSkyboxColor());
        }
    }

    IEnumerator ChangeSkyboxColor()
    {
        Color initialColor = currentSkybox.GetColor("_TintColor");
        float elapsedTime = 0f;
        Debug.Log("Changing skybox color to dawn...");
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            Color newColor = Color.Lerp(initialColor, dawnColor, t);
            currentSkybox.SetColor("_TintColor", newColor);
            currentSkybox.SetFloat("_Exposure", Mathf.Lerp(nightExposure, dawnExposure, t));
            yield return null;
        }

        currentSkybox.SetColor("_TintColor", dawnColor);
    }

    public void ResetSkybox()
    {
        currentSkybox.SetColor("_TintColor", nightColor);
        currentSkybox.SetFloat("_Exposure", nightExposure);
    }

    public void StartSkyboxTransition()
    {
        StartCoroutine(ChangeSkyboxColor());
    }
}

[CustomEditor(typeof(SkyboxManager))]
public class customButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SkyboxManager skyboxManager = (SkyboxManager)target;
        if (GUILayout.Button("Start Transition"))
        {
            skyboxManager.StartSkyboxTransition();
        }
        if (GUILayout.Button("Reset Skybox"))
        {
            skyboxManager.ResetSkybox();
        }
    }

}
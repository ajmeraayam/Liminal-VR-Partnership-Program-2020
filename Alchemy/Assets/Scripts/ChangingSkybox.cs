using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingSkybox : MonoBehaviour
{
    public Material[] skyboxMaterials;
    private int arraySize;
    private int currentSkyboxIndex;
    public float transitionTime;

    // Start is called before the first frame update
    void Start()
    {
        arraySize = skyboxMaterials.Length;
        currentSkyboxIndex = 0;
        RenderSettings.skybox = skyboxMaterials[currentSkyboxIndex]; 
        RenderSettings.skybox.SetFloat("_Exposure", 1f);
        transitionTime = 1f;
    }

    // For game, pass level as parameter
    public void Change()
    {
        StartCoroutine(TransitionSkybox());
    }

    IEnumerator TransitionSkybox()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(1, 0.1f, Mathf.Clamp01(elapsedTime / transitionTime));
            RenderSettings.skybox.SetFloat("_Exposure", currentAlpha);
            yield return new WaitForEndOfFrame();
        }

        currentSkyboxIndex++;

        if(currentSkyboxIndex >= arraySize)
        {
            currentSkyboxIndex = 0;
            RenderSettings.skybox = skyboxMaterials[currentSkyboxIndex];
        }
        else
        {
            RenderSettings.skybox = skyboxMaterials[currentSkyboxIndex];
        }
        RenderSettings.skybox.SetFloat("_Exposure", 0.1f);

        // https://answers.unity.com/questions/930780/setting-skybox-exposure-through-script.html
        elapsedTime = 0.0f;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(0.1f, 1, Mathf.Clamp01(elapsedTime / transitionTime));
            RenderSettings.skybox.SetFloat("_Exposure", currentAlpha);
            yield return new WaitForEndOfFrame();
        }
    }
}

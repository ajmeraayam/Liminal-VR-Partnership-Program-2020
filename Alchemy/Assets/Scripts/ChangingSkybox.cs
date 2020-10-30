using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingSkybox : MonoBehaviour
{
    // Store the skyboxes for each level. Skyboxes added to the array through inspector (DON'T try to add through scripts. May create conflicts)
    public Material[] skyboxMaterials;
    private int currentSkyboxIndex;
    // Transition time is total time of transition. i.e. Fading old skybox and bringing up new skybox
    public float transitionTime;

    // Start is called before the first frame update
    void Start()
    {
        currentSkyboxIndex = 0;
        // Select the default skybox which corresponds to level 1 and set its exposure to 1
        RenderSettings.skybox = skyboxMaterials[currentSkyboxIndex]; 
        RenderSettings.skybox.SetFloat("_Exposure", 1f);
        transitionTime = 1f;
    }

    // This method triggers the transition coroutine to change the skybox
    public void Change(int level)
    {
        StartCoroutine(TransitionSkybox(level));
    }

    // Removes the current skybox and adds a new skybox depending on the level in the game
    IEnumerator TransitionSkybox(int level)
    {
        float elapsedTime = 0.0f;
        // Gradually reduce the exposure of the current skybox 
        while (elapsedTime < (transitionTime/2))
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(1, 0.1f, Mathf.Clamp01(elapsedTime / (transitionTime/2)));
            RenderSettings.skybox.SetFloat("_Exposure", currentAlpha);
            yield return new WaitForEndOfFrame();
        }

        /*currentSkyboxIndex++;

        if(currentSkyboxIndex >= arraySize)
        {
            currentSkyboxIndex = 0;
            RenderSettings.skybox = skyboxMaterials[currentSkyboxIndex];
        }
        else
        {
            RenderSettings.skybox = skyboxMaterials[currentSkyboxIndex];
        }*/
        // Select the skybox according to the current level
        RenderSettings.skybox = skyboxMaterials[level - 1];
        RenderSettings.skybox.SetFloat("_Exposure", 0.1f);

        // https://answers.unity.com/questions/930780/setting-skybox-exposure-through-script.html
        elapsedTime = 0.0f;
        // Gradually increase the exposure of the new skybox
        while (elapsedTime < (transitionTime/2))
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(0.1f, 1, Mathf.Clamp01(elapsedTime / (transitionTime/2)));
            RenderSettings.skybox.SetFloat("_Exposure", currentAlpha);
            yield return new WaitForEndOfFrame();
        }
    }
}

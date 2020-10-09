using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public ParabolaAnimation ingredientScript;
    public ParticleSystem ingredientParticleSystem;
    public ParticleSystem splashParticleSystem;
    private ControllerScript controllerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        //Cache the controller script to access it quickly
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        // Find the script in children and disable it
        ingredientScript = GetComponentInChildren<ParabolaAnimation>();
        // Get particle systems for ingredient movement and splash effect after ingredient animation is complete
        foreach(Transform child in gameObject.transform)
        {
            if(child.tag == "Splash")
                splashParticleSystem = child.GetComponent<ParticleSystem>();

            if(child.tag == "Ingredient")
                ingredientParticleSystem = child.GetComponent<ParticleSystem>();
        }

        //ingredientScript.enabled = false;
        ingredientParticleSystem.Stop();
        splashParticleSystem.Stop();
    }

    public void StartAnimation()
    {
        ingredientParticleSystem.Play();
        ingredientScript.StartAnimation();
        //Send message to disable inputs from the controller when ladle is moving
        controllerScript.SendMessage("DisableInput", true);
        controllerScript.SendMessage("DisableRecipeCoroutine", true);
    }

    public void StopAnimation()
    {
        ingredientParticleSystem.Stop();
        splashParticleSystem.Play();
        //Send message to disable inputs from the controller when ladle is moving
        controllerScript.SendMessage("DisableInput", false);
        controllerScript.SendMessage("DisableRecipeCoroutine", false);
        ResetIngredientLocation();
    }

    public void ResetIngredientLocation()
    {
        ingredientScript.ResetLocation();
    }
}

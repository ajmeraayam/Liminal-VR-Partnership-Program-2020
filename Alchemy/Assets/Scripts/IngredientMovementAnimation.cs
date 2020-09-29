using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientMovementAnimation : MonoBehaviour
{
    // Array of all the waypoints for this basket
    public Transform[] waypoints;
    // Getter for waypoints
    public Transform[] Waypoints { get { return waypoints; } }
    // Ingredient Movement script attained from the children
    public IngredientMovement ingredientScript;
    public ParticleSystem ingredientParticleSystem;
    public ParticleSystem splashParticleSystem;
    private ControllerScript controllerScript;

    void Start()
    {
        //Cache the controller script to access it quickly
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        // Find the script in children and disable it
        ingredientScript = GetComponentInChildren<IngredientMovement>();
        // Get particle systems for ingredient movement and splash effect after ingredient animation is complete
        foreach(Transform child in gameObject.transform)
        {
            if(child.tag == "Splash")
                splashParticleSystem = child.GetComponent<ParticleSystem>();

            if(child.tag == "Ingredient")
                ingredientParticleSystem = child.GetComponent<ParticleSystem>();
        }

        ingredientScript.enabled = false;
        ingredientParticleSystem.Stop();
        splashParticleSystem.Stop();
        // Disable colliders on the waypoints
        EnableWaypoints(false);
    }

    public void StartAnimation()
    {
        // For animation, enable colliders and the script for movement
        EnableWaypoints(true);
        ingredientScript.enabled = true;
        ingredientParticleSystem.Play();
        //Send message to disable inputs from the controller when ladle is moving
        controllerScript.SendMessage("DisableInput", true);
        controllerScript.SendMessage("DisableRecipeCoroutine", true);
    }

    public void StopAnimation()
    {
        // Disable colliders and movement script when animation complete
        EnableWaypoints(false);
        ingredientScript.enabled = false;
        // Stop ingredient particle system and start the splashing effect
        ingredientParticleSystem.Stop();
        splashParticleSystem.Play();
        //Send message to disable inputs from the controller when ladle is moving
        controllerScript.SendMessage("DisableInput", false);
        controllerScript.SendMessage("DisableRecipeCoroutine", false);
        ResetIngredientLocation();
        ingredientScript.ResetInternalVariables();
    }

    private void EnableWaypoints(bool value)
    {
        // Enable or disable the colliders for all the waypoints in the array
        foreach(Transform waypoint in waypoints)
        {
            waypoint.GetComponent<SphereCollider>().enabled = value;
        }
    }

    public void ResetIngredientLocation()
    {
        ingredientScript.ResetLocation();
    }
}

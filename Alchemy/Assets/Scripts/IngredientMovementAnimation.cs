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
    public IngredientMovement script;
    private ControllerScript controllerScript;

    void Start()
    {
        //Cache the controller script to access it quickly
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        // Find the script in children and disable it
        script = GetComponentInChildren<IngredientMovement>();
        script.enabled = false;
        // Disable colliders on the waypoints
        EnableWaypoints(false);
    }

    public void StartAnimation()
    {
        // For animation, enable colliders and the script for movement
        EnableWaypoints(true);
        script.enabled = true;
        //Send message to disable inputs from the controller when ladle is moving
        controllerScript.SendMessage("DisableInput", true);
    }

    public void StopAnimation()
    {
        // Disable colliders and movement script when animation complete
        EnableWaypoints(false);
        script.enabled = false;
        //Send message to disable inputs from the controller when ladle is moving
        controllerScript.SendMessage("DisableInput", false);
    }

    private void EnableWaypoints(bool value)
    {
        // Enable or disable the colliders for all the waypoints in the array
        foreach(Transform waypoint in waypoints)
        {
            waypoint.GetComponent<SphereCollider>().enabled = value;
        }
    }
}

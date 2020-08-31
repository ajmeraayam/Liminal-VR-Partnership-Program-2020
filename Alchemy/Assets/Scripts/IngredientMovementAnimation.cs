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


    void Start()
    {
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
    }

    public void StopAnimation()
    {
        // Disable colliders and movement script when animation complete
        EnableWaypoints(false);
        script.enabled = false;
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

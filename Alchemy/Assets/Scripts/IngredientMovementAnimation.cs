using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientMovementAnimation : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform[] Waypoints { get { return waypoints; } }
    public IngredientMovement script;

    void Start()
    {
        script = GetComponentInChildren<IngredientMovement>();
        script.enabled = false;
    }

    public void StartAnimation()
    {
        script.enabled = true;
    }
}

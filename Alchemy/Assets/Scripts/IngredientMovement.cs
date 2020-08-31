using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientMovement : MonoBehaviour
{
    // The rate at which the gameobject will accelerate to the next waypoint. It will stop accelerating when speed limit is reached
    [Tooltip("The rate at which the gameobject will accelerate to the next waypoint. It will stop accelerating when speed limit is reached.")] public float acceleration = 0.8f;
    // The rate of deceleration. Lower values will bring gameobject to a faster halt. Value of 1.0 will never stop. Values greater than 1.0 will accelerate more. 
    [Tooltip("The rate of deceleration. Lower values will bring gameobject to a faster halt. Value of 1.0 will never stop. Values greater than 1.0 will accelerate more.")] public float inertia = 0.9f;
    // Maximum speed to accelerate to
    [Tooltip("Maximum speed to accelerate to.")] public float speedLimit = 10f;
    // When this speed is reached, the gameobject will stop moving.
    [Tooltip("When this speed is reached, the gameobject will stop moving.")] public float minSpeed = 1f;
    // Amount of time the gameobject should wait before it starts moving towards next waypoint. 
    [Tooltip("Amount of time the gameobject should wait before it starts moving towards next waypoint.")] public float stopTime = 1f;
    // Gameobject that should move through waypoints
    //[Tooltip("Gameobject that should move through waypoints")] public GameObject flyingGameObject;
    // Speed of gameobject. This variable is updated each frame and is updated by acceleration and inertia.
    private float currentSpeed = 0f;
    // This variable controls if gameobject should accelerate or stop.
    private bool functionState = true;
    // True when acceleration should be active.
    private bool accelerateState;
    // True when deceleration should be active.
    private bool slowState;
    // This variable stores the next transform this gameobject should move to. 
    private Transform waypoint;
    // Rotation speed
    public float rotationDamping = 6f;
    // True if gameobject should rotate smoothly. False if gameobject should rotate instantly
    [Tooltip("True if gameobject should rotate smoothly. False if gameobject should rotate instantly")] public bool smoothRotation = true;
    // Array contains all the waypoints this gameobject should go through.
    [Tooltip("Array contains all the waypoints this gameobject should go through.")] public Transform[] waypoints;
    // Points to which waypoint is currently active 
    private int waypointIndexPointer = 0;
    
    void Start()
    {
        functionState = true;
        waypointIndexPointer = 0;
        waypoints = GetComponentInParent<WaypointList>().Waypoints;
    }

    void Update()
    {
        if(waypointIndexPointer <= 2)
        {
            waypoint = waypoints[waypointIndexPointer];
            // Accelerate if functionState is TRUE
            if(functionState)
            {
                Accelerate();
            }
            // Decelerate if functionState is FALSE
            else
            {
                Decelerate();
            }
            // Keep the gameobject pointed towards the next/active waypoint
            //waypoint = waypoints[waypointIndexPointer];
        }
    }

    void OnTriggerEnter()
    {
        // When gameobject hits the waypoint's collider, start deceleration and activate next waypoint 
        functionState = false;
        waypointIndexPointer++;
    }

    private void Accelerate()
    {
        if(!accelerateState)
        {
            // Acceleration should work and deceleration should not
            accelerateState = true;
            slowState = false;
        }

        // If there is a waypoint
        /*if(waypoint)
        {
            // Rotate the gameobject towards the waypoint
            var rotation = Quaternion.LookRotation(waypoint.position - flyingGameObject.transform.position);
            // Smoothen the rotation
            flyingGameObject.transform.rotation = Quaternion.Slerp(flyingGameObject.transform.rotation, rotation, Time.deltaTime * rotationDamping);
        }*/
        // Accelerate towards waypoint. Accelerate until speed limit reached
        currentSpeed += acceleration * acceleration;
        print(waypoint);
        //print(flyingGameObject);
        Vector3 distance = Vector3.Normalize(waypoint.position - transform.position);
        //flyingGameObject.transform.Translate(0, 0, Time.deltaTime * currentSpeed);
        transform.Translate(distance * currentSpeed * Time.deltaTime);

        // If current speed exceed speed limit, then clamp it to speed limit
        if(currentSpeed >= speedLimit)
        {
            currentSpeed = speedLimit;
        }
    }

    private void Decelerate()
    {
        if(slowState)
        {
            // Deceleration should work and acceleration should not
            accelerateState = true;
            slowState = false;
        }
        // Start to slow down.
        currentSpeed *= inertia;
        transform.Translate(0, 0, Time.deltaTime * currentSpeed);

        if(currentSpeed <= minSpeed)
        {
            currentSpeed = 0f;
            // Add time delay here 
            functionState = true;
        } 
    }


}

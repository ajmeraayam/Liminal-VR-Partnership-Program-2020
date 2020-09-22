using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientMovement : MonoBehaviour
{
    private Vector3 initialLocalPos;
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
    public int WaypointIndexPointer { set { waypointIndexPointer = value; } get { return waypointIndexPointer; } }
    bool delayEnabled = false;
    float delayTimeElapsed;
    AudioSource source;
    public AudioClip pickupClip;
    public AudioClip whooshClip;
    public AudioClip splashClip;
    private bool whooshPlayed, pickupPlayed;
    private IngredientMovementAnimation movAnimationScript;
    
    void Start()
    {
        delayTimeElapsed = 0f;
        functionState = true;
        waypointIndexPointer = 0;
        movAnimationScript = GetComponentInParent<IngredientMovementAnimation>();
        waypoints = movAnimationScript.Waypoints;
        source = GetComponent<AudioSource>();
        whooshPlayed = false;
        pickupPlayed = false;
        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        if(delayEnabled)
        {
            if(delayTimeElapsed < stopTime)
            {
                delayTimeElapsed += Time.deltaTime;
                return;
            }
            else
            {
                delayTimeElapsed = 0f;
                delayEnabled = false;
                // Play whoosh audio clip when moving through waypoints
                if((waypointIndexPointer == 1 || waypointIndexPointer == 2) && !whooshPlayed)
                {
                    source.PlayOneShot(whooshClip);
                    whooshPlayed = true;
                }
            }
        }

        if(waypointIndexPointer <= 2)
        {
            // Play pickup audio clip
            if(waypointIndexPointer == 0 && !pickupPlayed)
            {
                source.PlayOneShot(pickupClip);
                pickupPlayed = true;
            }
            // Keep the gameobject pointed towards the next/active waypoint
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
        }
        else
        {
            // Send message to parent
            waypointIndexPointer = 0;
            source.PlayOneShot(splashClip);
            movAnimationScript.StopAnimation();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        // When gameobject hits the waypoint's collider, start deceleration and activate next waypoint 
        functionState = false;
        // Prevent waypoint index pointer to increment when gameobject collides multiple times with same waypoint
        if(other.name == waypoint.name)
        {
            waypointIndexPointer++;
            pickupPlayed = false;
            whooshPlayed = false;
        }
    }

    private void Accelerate()
    {
        if (!accelerateState)
        {
            // Acceleration should work and deceleration should not
            accelerateState = true;
            slowState = false;
            
        }

        // Accelerate towards waypoint. Accelerate until speed limit reached
        currentSpeed += acceleration * acceleration;
        Vector3 distance = Vector3.Normalize(waypoint.position - transform.position);
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
            if(waypointIndexPointer <= 2)
                delayEnabled = true;
            functionState = true;
        } 
    }

    public void ResetLocation()
    {
        transform.localPosition = initialLocalPos;
    }
}

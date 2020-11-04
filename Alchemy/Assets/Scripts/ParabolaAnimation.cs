using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaAnimation : MonoBehaviour
{
    // Initial location of the ingredient (local position w.r.t. parent game object)
    private Vector3 initialLocalPos;
    // Target position
    public Transform target;
    [Range(10f, 80f)] public float firingAngle = 45.0f;
    [Range(0f, 100f)][Tooltip("in m/s")]public float gravity = 9.8f;
    // Transform for the object that should be thrown (in this case the ingredient itself)
    public Transform projectile;
    // Start location of the ingredient (world coordinates)
    private Transform startLocTransform;
    // Reference to AnimationHandler script
    private AnimationHandler animationHandler;
    public AudioClip pickupClip;
    public AudioClip splashClip;
    AudioSource source;

    void Awake()
    {
        startLocTransform = transform;
    }

    void Start()
    {
        animationHandler = GetComponentInParent<AnimationHandler>();
        source = GetComponent<AudioSource>();
        initialLocalPos = transform.localPosition;
    }
    
    // Trigger the parabola motion animation
    public void StartAnimation()
    {
        StartCoroutine(SimulateProjectile());
    }

    // Gradually changes the position of the ingredient which imitates a parabolic motion
    IEnumerator SimulateProjectile()
    {
        source.PlayOneShot(pickupClip);
        yield return new WaitForSeconds(0.1f);
        projectile.position = startLocTransform.position + new Vector3(0f, 0f, 0f);
        float target_distance = Vector3.Distance(projectile.position, target.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X-Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float flightDuration = target_distance / Vx;
        // Change the direction of the ingredient towards the target position
        projectile.rotation = Quaternion.LookRotation(target.position - projectile.position);

        float elapsedTime = 0f;

        // Loop until ingredient reaches the target
        while(elapsedTime < flightDuration)
        {
            projectile.Translate(0, (Vy - (gravity * elapsedTime)) * Time.deltaTime, Vx * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        source.PlayOneShot(splashClip);
        animationHandler.StopAnimation();
    }

    // Trigger the resetLoc function
    public void ResetLocation()
    {
        StartCoroutine(resetLoc());
    }

    // Place the ingredients back to its initial local position after waiting for 1 second from the method call
    private IEnumerator resetLoc()
    {
        yield return new WaitForSeconds(1f);
        transform.localPosition = initialLocalPos;
    }
}
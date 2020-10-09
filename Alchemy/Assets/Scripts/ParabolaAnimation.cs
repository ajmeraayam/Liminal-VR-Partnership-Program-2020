using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaAnimation : MonoBehaviour
{
    private Vector3 initialLocalPos;
    public Transform target;
    [Range(10f, 80f)] public float firingAngle = 45.0f;
    [Range(0f, 100f)][Tooltip("in m/s")]public float gravity = 9.8f;

    public Transform projectile;
    private Transform startLocTransform;
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
 
    public void StartAnimation()
    {
        StartCoroutine(SimulateProjectile());
    }

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

        projectile.rotation = Quaternion.LookRotation(target.position - projectile.position);

        float elapsedTime = 0f;

        while(elapsedTime < flightDuration)
        {
            projectile.Translate(0, (Vy - (gravity * elapsedTime)) * Time.deltaTime, Vx * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        source.PlayOneShot(splashClip);
        animationHandler.StopAnimation();
    }

    public void ResetLocation()
    {
        StartCoroutine(resetLoc());
    }

    private IEnumerator resetLoc()
    {
        yield return new WaitForSeconds(1f);
        transform.localPosition = initialLocalPos;
    }
}
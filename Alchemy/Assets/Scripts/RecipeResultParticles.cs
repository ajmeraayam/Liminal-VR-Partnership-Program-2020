using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeResultParticles : MonoBehaviour
{
    private Transform successObject, failObject;
    private ControllerScript controllerScript;
    private ParticleSystem success;
    private ParticleSystem fail;

    void Start()
    {
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        successObject = transform.Find("PoofSuccess");
        failObject = transform.Find("PoofFail");
        success = successObject.GetComponent<ParticleSystem>();
        fail = failObject.GetComponent<ParticleSystem>();
    }

    public void PlaySuccessParticles()
    {
        success.Play(true);
    }

    public void PlayFailureParticles()
    {
        fail.Play(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeResultParticles : MonoBehaviour
{
    public ParticleSystem success;
    public ParticleSystem fail;
    private ControllerScript controllerScript;
    void Start()
    {
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
    }

    public void PlaySuccessParticles()
    {
        success.Play();
    }

    public void PlayFailureParticles()
    {
        fail.Play();
    }
}

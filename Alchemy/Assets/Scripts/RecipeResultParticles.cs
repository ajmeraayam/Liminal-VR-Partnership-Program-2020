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
        controllerScript.SendMessage("DisableInput", true);
        StartCoroutine(SuccessParticles());
    }

    public void PlayFailureParticles()
    {
        controllerScript.SendMessage("DisableInput", true);
        StartCoroutine(FailureParticles());
    }

    IEnumerator SuccessParticles()
    {
        yield return new WaitForSeconds(0.5f);
        success.Play();
        controllerScript.SendMessage("DisableInput", false);
    }

    IEnumerator FailureParticles()
    {
        yield return new WaitForSeconds(0.5f);
        fail.Play();
        controllerScript.SendMessage("DisableInput", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGenerator : MonoBehaviour
{
    public ParticleSystem particle;
    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            particle.Play();
        }*/
    }

    public void startFire()
    {
        particle.Play();
    }
}

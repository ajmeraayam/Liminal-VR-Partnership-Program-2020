using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGenerator : MonoBehaviour
{
    public ParticleSystem particle;
    private AudioClip fireFlick;
    private AudioSource source;

    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        source = GetComponent<AudioSource>();
        fireFlick = Resources.Load<AudioClip>("flick");
        source.clip = fireFlick;
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
        source.PlayOneShot(source.clip);
        particle.Play();
    }
}

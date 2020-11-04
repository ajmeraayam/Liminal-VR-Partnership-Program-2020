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

    // Start the fire particle system
    public void startFire()
    {
        source.PlayOneShot(source.clip);
        particle.Play();
    }
}

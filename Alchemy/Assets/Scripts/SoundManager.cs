using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // pick up sounds
    public static AudioClip herbPick, mushroomPick, mineralPick, magicPick;

    // pot sounds
    public static AudioClip flick, splash, stir;

    // level sounds
    public static AudioClip levelUp;

    // other sounds
    public static AudioClip whooshSound;

    public static AudioSource source;

    void Start()
    {
        // pickup sounds, refer to Controller Script as it is maintained to direct interaction
        herbPick = Resources.Load<AudioClip>("herb pickup");
        mushroomPick = Resources.Load<AudioClip>("mushroom pickup");
        mineralPick = Resources.Load<AudioClip>("mineral pickup");
        magicPick = Resources.Load<AudioClip>("magic pickup");

        // pot sounds
        splash = Resources.Load<AudioClip>("splash");
        stir = Resources.Load<AudioClip>("stir");
        flick = Resources.Load<AudioClip>("flick");

        // level sounds
        levelUp = Resources.Load<AudioClip>("level up");

        // other sounds
        whooshSound = Resources.Load<AudioClip>("whoosh");

        source = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Play (string file)
    {
        switch (file)
        {
            // pickup sounds, refer to controller script
            case "herb pickup":
                source.PlayOneShot(herbPick);
                break;
            case "mushroom pickup":
                source.PlayOneShot(mushroomPick);
                break;
            case "mineral pickup":
                source.PlayOneShot(mineralPick);
                break;
            case "magic pickup":
                source.PlayOneShot(magicPick);
                break;

            // pot sounds
            case "splash":
                source.PlayOneShot(splash);
                break;
            case "stir": // *REFER to Controller
                source.PlayOneShot(stir);
                break;
            case "flick": // *REFER to Controller
                source.PlayOneShot(flick);
                break;

            // other sounds
            case "whoosh": // *REFER to Ingredient Movement
                source.PlayOneShot(whooshSound);
                break;

            // level sounds
            case "levelUp":
                source.PlayOneShot(levelUp);
                break;
        }
    }
}

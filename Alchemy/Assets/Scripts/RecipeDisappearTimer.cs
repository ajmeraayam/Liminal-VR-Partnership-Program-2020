using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDisappearTimer : MonoBehaviour
{
    //Suppose the player has 30 seconds to complete the recipe
    private int timer = 0;
    private int level;
    // TRUE if timer is complete else FALSE
    private bool timerComplete;
    private ControllerScript controllerScript;
    private Timer timerDisplayScript;
    private RecipeDisplayManager recipeDisplayManager;
    // Refernce to the timer coroutine
    private Coroutine routine = null;

    void Awake()
    {
        timerDisplayScript = GameObject.Find("Timer Disappear").GetComponent<Timer>();
        recipeDisplayManager = GameObject.Find("Game Manager").GetComponent<RecipeDisplayManager>();
    }

    void Start()
    {
        level = 1;
        timerComplete = false;
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        timer = 8;
        timerDisplayScript.SetDuration((float)timer);
    }

    // Update the level
    public void UpdateLevel(int l)
    {
        level = l;
    }

    // Reset the timer according to the level
    private void ResetTimer()
    {
        if (level == 3)
        {
            timer = 15;
            // Set display to show sprite according to the duration
            timerDisplayScript.SetDuration((float)timer);
        }
        else if (level == 4)
        {
            timer = 10;
            // Set display to show sprite according to the duration
            timerDisplayScript.SetDuration((float)timer);
        }
        else if (level == 5)
        {
            timer = 7;
            // Set display to show sprite according to the duration
            timerDisplayScript.SetDuration((float)timer);
        }
    }

    // Trigger the coroutine if the current level is above level 2 and the timer isn't already running
    public void StartTimer()
    {
        if (level > 2)
            routine = StartCoroutine(TimerCoroutine());
    }

    // Stop the ongoing timer coroutine and reset the timer.
    public void StopTimer()
    {
        if (level > 2)
        {
            StopCoroutine(routine);
            ResetTimer();
        }
    }

    // Coroutine that resembles a clock
    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(2f);
        // Send the timer value to the display board
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
            //Send the timer value to the display board
            timerDisplayScript.Countdown((float)timer);
            if (timer <= 0)
                break;
        }
        // Remove recipe from board
        recipeDisplayManager.DisappearRecipe();
        ResetTimer();
    }
}
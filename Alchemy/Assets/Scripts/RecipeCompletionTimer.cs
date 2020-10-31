using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCompletionTimer : MonoBehaviour
{
    //Suppose the player has 30 seconds to complete the recipe
    private int timer;
    private Timer timerDisplayScript;
    // Current level
    private int level;
    // TRUE if timer is complete else FALSE
    private bool timerComplete;
    private ControllerScript controllerScript;
    private RecipeDisappearTimer disappearTimer;
    // TRUE if timer is on, else FALSE
    private bool isRunning = false;
    // Refernce to the timer coroutine
    private Coroutine routine = null;

    void Awake()
    {
        timerDisplayScript = GameObject.Find("Timer").GetComponent<Timer>();
    }

    void Start()
    {
        // Initially level 1
        level = 1;
        timerComplete = false;
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        timer = 30;
        timerDisplayScript.SetDuration((float)timer);
        disappearTimer = GetComponent<RecipeDisappearTimer>();
    }

    // Update the level
    public void UpdateLevel(int l)
    {
        level = l;
    }

    // Reset the timer according to the level
    // The aim is to give the user double the time after the recipe disappears
    public void ResetTimer()
    {
        if (level == 2)
        {
            timer = 30;
            // Set display to show sprite according to the duration
            timerDisplayScript.SetDuration((float)timer);
        }
        else if (level == 3)
        {
            timer = 30;
            // Set display to show sprite according to the duration
            timerDisplayScript.SetDuration((float)timer);
        }
        else if (level == 4)
        {
            timer = 25;
            // Set display to show sprite according to the duration
            timerDisplayScript.SetDuration((float)timer);
        }
        else if (level == 5)
        {
            timer = 20;
            // Set display to show sprite according to the duration
            timerDisplayScript.SetDuration((float)timer);
        }
        isRunning = false;
    }

    // Trigger the coroutine if the current level is above level 1 and the timer isn't already running
    public void StartTimer()
    {
        if (level > 1 && !isRunning)
        {
            routine = StartCoroutine(TimerCoroutine());
        }
    }

    // Stop the ongoing timer coroutine and reset the timer. Also stop the disappear timer if current level is above level 2
    public void StopTimer()
    {
        if (level > 1)
        {
            StopCoroutine(routine);
            ResetTimer();
            if (level > 2)
            {
                disappearTimer.StopTimer();
            }
        }
    }

    // Coroutine that resembles a clock
    IEnumerator TimerCoroutine()
    {
        isRunning = true;
        yield return new WaitForSeconds(2f);
        disappearTimer.StartTimer();
        // Send the timer value to the display board
        while (true)
        {
            timerDisplayScript.Countdown((float)timer);
            yield return new WaitForSeconds(1f);
            timer--;
            //Send the timer value to the display board
            if (timer <= 0)
                break;
        }

        controllerScript.SendMessage("OnCompletionTimerEnd");
    }
}
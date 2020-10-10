using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCompletionTimer : MonoBehaviour
{
    //Suppose the player has 30 seconds to complete the recipe
    private int timer;
    private Timer timerDisplayScript;
    private int level;
    private bool timerComplete;
    private ControllerScript controllerScript;
    private RecipeDisappearTimer disappearTimer;
    private bool isRunning = false;
    private Coroutine routine = null;

    void Awake()
    {
        timerDisplayScript = GameObject.Find("Timer").GetComponent<Timer>();
    }

    void Start()
    {
        level = 1;
        timerComplete = false;
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        timer = 60;
        timerDisplayScript.SetDuration((float) timer);
        disappearTimer = GetComponent<RecipeDisappearTimer>();
    }

    public void UpdateLevel(int l)
    {
        level = l;
    }

    public void ResetTimer()
    {
        if(level == 2)
        {
            timer = 60;
            timerDisplayScript.SetDuration((float) timer);
        }
        else if(level == 3)
        {
            timer = 55;
            timerDisplayScript.SetDuration((float) timer);
        }
        else if(level == 4)
        {
            timer = 50;
            timerDisplayScript.SetDuration((float) timer);
        }
        else if(level == 5)
        {
            timer = 45;
            timerDisplayScript.SetDuration((float) timer);
        }
        isRunning = false;
    }

    public void StartTimer()
    {
        if(level > 1 && !isRunning)
        {
            routine = StartCoroutine(TimerCoroutine());
        }
    }

    public void StopTimer()
    {
        if(level > 1)
        {
            StopCoroutine(routine);
            ResetTimer();
        }
    }

    IEnumerator TimerCoroutine()
    {
        isRunning = true;
        yield return new WaitForSeconds(2f);
        disappearTimer.StartTimer();
        // Send the timer value to the display board
        while(true)
        {
            timerDisplayScript.Countdown((float) timer);
            yield return new WaitForSeconds(1f);
            timer--;
            //Send the timer value to the display board
            if(timer <= 0)
                break;
        }

        controllerScript.SendMessage("OnCompletionTimerEnd");
    }
}

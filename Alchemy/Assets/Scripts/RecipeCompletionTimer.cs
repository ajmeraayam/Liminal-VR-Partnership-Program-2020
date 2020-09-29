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

    void Awake()
    {
        timerDisplayScript = GameObject.Find("Timer").GetComponent<Timer>();
    }

    void Start()
    {
        level = 1;
        timerComplete = false;
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        timer = 30;
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
            timer = 30;
            timerDisplayScript.SetDuration((float) timer);
        }
        else if(level == 3)
        {
            timer = 25;
            timerDisplayScript.SetDuration((float) timer);
        }
        else if(level == 4)
        {
            timer = 20;
            timerDisplayScript.SetDuration((float) timer);
        }
        else if(level == 5)
        {
            timer = 20;
            timerDisplayScript.SetDuration((float) timer);
        }
    }

    public void StartTimer()
    {
        if(level > 1)
            StartCoroutine(TimerCoroutine());
    }

    public void StopTimer()
    {
        if(level > 1)
        {
            StopCoroutine(TimerCoroutine());
            ResetTimer();
        }
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        disappearTimer.StartTimer();
        // Send the timer value to the display board
        while(true)
        {
            //print("Timer - " + timer);
            timerDisplayScript.Countdown((float) timer);
            yield return new WaitForSeconds(1f);
            timer -= 1;
            //Send the timer value to the display board
            if(timer <= 0)
                break;
        }

        controllerScript.SendMessage("StartFailedRecipeCoroutine");
        
    }
}

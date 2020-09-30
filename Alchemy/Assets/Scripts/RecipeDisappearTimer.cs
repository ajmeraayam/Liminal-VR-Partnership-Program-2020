using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDisappearTimer : MonoBehaviour
{
    //Suppose the player has 30 seconds to complete the recipe
    private int timer = 0;
    private int level;
    private bool timerComplete;
    private ControllerScript controllerScript;
    private Timer timerDisplayScript;
    private RecipeDisplayManager recipeDisplayManager;
    
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
        timerDisplayScript.SetDuration((float) timer);
    }

    public void UpdateLevel(int l)
    {
        level = l;
    }

    private void ResetTimer()
    {
        if(level == 3)
        {
            timer = 8;
            timerDisplayScript.SetDuration((float) timer);
        }
        else if(level == 4)
        {
            timer = 5;
            timerDisplayScript.SetDuration((float) timer);
        }
        else if(level == 5)
        {
            timer = 4;
            timerDisplayScript.SetDuration((float) timer);
        }
    }

    public void StartTimer()
    {
        if(level > 2)
            StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(2f);
        // Send the timer value to the display board
        while(true)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
            //Send the timer value to the display board
            timerDisplayScript.Countdown((float) timer);
            if(timer <= 0)
                break;
        }
        // Remove recipe from board
        recipeDisplayManager.DisappearRecipe();
        ResetTimer();
    }
}

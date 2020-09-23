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
    
    void Start()
    {
        level = 1;
        timerComplete = false;
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        timer = 8;
    }

    public void UpdateLevel(int l)
    {
        level = l;
    }

    private void ResetTimer()
    {
        if(level == 3)
            timer = 8;
        else if(level == 4)
            timer = 5;
        else if(level == 5)
            timer = 4;
    }

    public void StartTimer()
    {
        if(level > 2)
            StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        // Send the timer value to the display board
        while(true)
        {
            //Send the timer value to the display board
            print("Disappear timer - " + timer);
            yield return new WaitForSeconds(1f);
            timer -= 1;
            if(timer <= 0)
                break;
        }
        print("Recipe disappear");
        ResetTimer();
        // Remove recipe from board
    }
}

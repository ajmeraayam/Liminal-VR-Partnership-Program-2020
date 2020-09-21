using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //Suppose the player has 30 seconds to complete the recipe
    float timer = 30f;
    int level;
    void Start(){
        
    }

    void Update(){
        //when the game come up to level 4 or higher than level 4, there will have a timer
        if (level >= 4) {
            timer -= Time.deltaTime;
            //when the time <= 0, the game fail
            if (timer <= 0) {

            }
        } 
    }
}

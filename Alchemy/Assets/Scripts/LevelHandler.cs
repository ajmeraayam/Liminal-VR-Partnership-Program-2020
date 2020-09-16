using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    // Stores the current level (1 to 5)
    private int currentRecipeLevel;
    private int correctStreak;
    private int consecWrongStreak;
    // TRUE if last recipe was correct, FALSE otherwise
    private bool lastRecipe;


    // Manages level of recipes to be generated
    public void changeRecipeLevel()
    {
        if(currentRecipeLevel == 1)
        {
            if(correctStreak == 3)
            {
                currentRecipeLevel++;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
        }
        else if(currentRecipeLevel == 2)
        {
            if(correctStreak == 5)
            {
                currentRecipeLevel++;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
            if(consecWrongStreak == 5)
            {
                currentRecipeLevel--;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
        }
        else if(currentRecipeLevel == 3)
        {
            if(correctStreak == 15)
            {
                currentRecipeLevel++;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
            if(consecWrongStreak == 5)
            {
                currentRecipeLevel--;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
        }
        else if(currentRecipeLevel == 4)
        {
            if(correctStreak == 20)
            {
                currentRecipeLevel++;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
            if(consecWrongStreak == 4)
            {
                currentRecipeLevel--;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
        }
        else if(currentRecipeLevel == 5)
        {
            if(consecWrongStreak == 3)
            {
                currentRecipeLevel--;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
        }
    }

    // Call this method when player makes a wrong recipe. This method will take care of streaks of potions
    public void wrongRecipe()
    {
        if(!lastRecipe)
        {
            consecWrongStreak++;
            lastRecipe = false;
        }
        else
        {
            consecWrongStreak = 1;
            correctStreak = 0;
            lastRecipe = false;
        }
    }

    // Call this method when player makes a correct recipe. This method will take care of streaks of potions
    public void correctRecipe()
    {
        if(lastRecipe)
        {
            correctRecipe++;
            lastRecipe = false;
        }
        else
        {
            consecWrongStreak = 0;
            correctRecipe = 1;
            lastRecipe = false;
        }
    }
}

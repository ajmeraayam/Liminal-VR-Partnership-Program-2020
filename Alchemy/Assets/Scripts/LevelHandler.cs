using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    // Stores the current level (1 to 5)
    private int currentRecipeLevel;
    private int correctStreak;
    private int score;
    public int CorrectStreak { get { return correctStreak;} }
    public int Score { get {return score; } }
    private int consecWrongStreak;
    // TRUE if last recipe was correct, FALSE otherwise
    private bool lastRecipe;
    private RecipeGenerator recipeGenerator;
    private RecipeCompletionTimer completionTimer;
    private RecipeDisappearTimer disappearTimer;
    private string[] recipe;
    bool isRecipeGeneratable;
    int maxActions;
    private GameObject timerDisplayObject;

    void Awake()
    {
        timerDisplayObject = GameObject.Find("Timer");
    }
    void Start()
    {
        recipeGenerator = GetComponent<RecipeGenerator>();
        completionTimer = GetComponent<RecipeCompletionTimer>();
        disappearTimer = GetComponent<RecipeDisappearTimer>();
        isRecipeGeneratable = true;
        currentRecipeLevel = 1;
        maxActions = 0;
        score = 0;
    }

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
        completionTimer.UpdateLevel(currentRecipeLevel);
        disappearTimer.UpdateLevel(currentRecipeLevel);
        EnableTimer(currentRecipeLevel);
    }

    private void EnableTimer(int level)
    {
        if(level < 2)
        {
            timerDisplayObject.SetActive(false);
        }
        else
        {
            if(!timerDisplayObject.activeInHierarchy)
            {
                timerDisplayObject.SetActive(true);
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
            changeRecipeLevel();
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
            correctStreak++;
            lastRecipe = true;
            changeRecipeLevel();
        }
        else
        {
            consecWrongStreak = 0;
            correctStreak = 1;
            lastRecipe = true;
        }
        score++;
    }
    
    public void generateNewRecipe()
    {
        if(isRecipeGeneratable)
        {
            recipe = recipeGenerator.GetRandomRecipe(currentRecipeLevel);
            isRecipeGeneratable = false;
            maxActions = recipe.Length;
        }
    }

    public string[] getCurrentRecipe()
    {
        return recipe;
    }

    public int getMaxActions()
    {
        return maxActions;
    }

    public void recipeGeneratable()
    {
        isRecipeGeneratable = true;
    }
}

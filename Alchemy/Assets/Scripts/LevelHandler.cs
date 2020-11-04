using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    // Stores the current level (1 to 5)
    private int currentRecipeLevel;
    public int CurrentRecipeLevel { get { return currentRecipeLevel; } }
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
    private GameObject completionTimerDisplayObject;
    private GameObject disappearTimerDisplayObject;
    public GameObject levelUpConfetti;
    private ParticleSystem levelUpParticles;
    public AudioClip levelUpClip;
    public AudioSource source;
    private ChangingSkybox skyboxScript;

    void Awake()
    {
        completionTimerDisplayObject = GameObject.Find("Timer");
        disappearTimerDisplayObject = GameObject.Find("Timer Disappear");
    }

    void Start()
    {
        recipeGenerator = GetComponent<RecipeGenerator>();
        completionTimer = GetComponent<RecipeCompletionTimer>();
        disappearTimer = GetComponent<RecipeDisappearTimer>();
        skyboxScript = GetComponent<ChangingSkybox>();
        isRecipeGeneratable = true;
        currentRecipeLevel = 1;
        maxActions = 0;
        score = 0;
        levelUpParticles = levelUpConfetti.GetComponent<ParticleSystem>();
    }

    // Manages level of recipes to be generated
    public void changeRecipeLevel()
    {
        if(currentRecipeLevel == 1)
        {
            // If 4 consecutive correct recipes then increase the level, reset all the variables and play level up animation 
            if(correctStreak == 4)
            {
                currentRecipeLevel++;
                correctStreak = 0;
                consecWrongStreak = 0;
                StartCoroutine(PlayLevelUpAnimation());
            }
        }
        else if(currentRecipeLevel == 2)
        {
            // If 5 consecutive correct recipes then increase the level, reset all the variables and play level up animation
            if(correctStreak == 5)
            {
                currentRecipeLevel++;
                correctStreak = 0;
                consecWrongStreak = 0;
                StartCoroutine(PlayLevelUpAnimation());
            }
            // If 4 consecutive wrong recipes then decrease the level and reset all the variables
            if(consecWrongStreak == 4)
            {
                currentRecipeLevel--;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
        }
        else if(currentRecipeLevel == 3)
        {
            // If 6 consecutive correct recipes then increase the level, reset all the variables and play level up animation
            if(correctStreak == 6)
            {
                currentRecipeLevel++;
                correctStreak = 0;
                consecWrongStreak = 0;
                StartCoroutine(PlayLevelUpAnimation());
            }
            // If 3 consecutive wrong recipes then decrease the level and reset all the variables
            if(consecWrongStreak == 3)
            {
                currentRecipeLevel--;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
        }
        else if(currentRecipeLevel == 4)
        {
            // If 7 consecutive correct recipes then increase the level, reset all the variables and play level up animation
            if(correctStreak == 7)
            {
                currentRecipeLevel++;
                correctStreak = 0;
                consecWrongStreak = 0;
                StartCoroutine(PlayLevelUpAnimation());
            }
            // If 3 consecutive wrong recipes then decrease the level and reset all the variables
            if(consecWrongStreak == 3)
            {
                currentRecipeLevel--;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
        }
        else if(currentRecipeLevel == 5)
        {
            // If 2 consecutive wrong recipes then decrease the level and reset all the variables
            if(consecWrongStreak == 2)
            {
                currentRecipeLevel--;
                correctStreak = 0;
                consecWrongStreak = 0;
            }
        }
        // Update all the timers
        completionTimer.UpdateLevel(currentRecipeLevel);
        disappearTimer.UpdateLevel(currentRecipeLevel);
        EnableCompletionTimer(currentRecipeLevel);
        EnableDisappearTimer(currentRecipeLevel);
    }

    // Enable completion timer if current level is greater than 1 and the timer is not active already
    private void EnableCompletionTimer(int level)
    {
        if(level < 2)
        {
            completionTimerDisplayObject.SetActive(false);
        }
        else
        {
            if(!completionTimerDisplayObject.activeInHierarchy)
            {
                completionTimerDisplayObject.SetActive(true);
            }
        }
    }

    // Enable disappear timer if current level is greater than 2 and the timer is not active already
    private void EnableDisappearTimer(int level)
    {
        if(level < 3)
        {
            disappearTimerDisplayObject.SetActive(false);
        }
        else
        {
            if(!disappearTimerDisplayObject.activeInHierarchy)
            {
                disappearTimerDisplayObject.SetActive(true);
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
    
    // Generates a new recipe according to the current level
    public void generateNewRecipe()
    {
        if(isRecipeGeneratable)
        {
            recipe = recipeGenerator.GetRandomRecipe(currentRecipeLevel);
            isRecipeGeneratable = false;
            maxActions = recipe.Length;
        }
    }

    // Returns all the recipe ingredients/actions for the current recipe
    public string[] getCurrentRecipe()
    {
        return recipe;
    }

    // Maximum number of actions/ingredients in the current recipe
    public int getMaxActions()
    {
        return maxActions;
    }

    // Toggles the recipe generator to let it generate a new recipe
    public void recipeGeneratable()
    {
        isRecipeGeneratable = true;
    }

    // Play the level up animation and change the skybox
    private IEnumerator PlayLevelUpAnimation()
    {
        yield return new WaitForSeconds(1f);
        skyboxScript.Change(currentRecipeLevel);
        levelUpParticles.Play();
        source.PlayOneShot(levelUpClip);
    }
}

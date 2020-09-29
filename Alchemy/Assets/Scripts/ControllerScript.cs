using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;

/*
 * Performance improvements - Cache methods for gameobjects (probably in hashmap) that are used often in the game. 
 * It will save the time to search for script in the gameobject. 
 * Another way is to check if OnPointerClick, OnPointerDown and OnPointerUp does the job better or not
*/
public class ControllerScript : MonoBehaviour
{
    //To disable input if needed
    bool disableInput = false;
    bool disableRecipeCoroutine = false;
    bool gameStarted = false;
    private GameObject gameManager;
    private LevelHandler levelHandler;
    private GameObject success_fail_particleSystem;
    private RecipeResultParticles particles;
    string[] currentRecipe;
    int actionsTaken;
    int maxActionsForThisRecipe;
    string[] recordedActions;
    public GameObject[] baskets;
    private IngredientMovementAnimation[] basketScripts;
    private RecipeCompletionTimer completionTimerScript;
    private RecipeDisplayManager recipeDisplayScript;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager");
        success_fail_particleSystem = GameObject.Find("Success Fail PS");
        levelHandler = gameManager.GetComponent<LevelHandler>();
        particles = success_fail_particleSystem.GetComponent<RecipeResultParticles>();
        actionsTaken = 0;
        maxActionsForThisRecipe = 0;
        recordedActions = new string[0];
        basketScripts = new IngredientMovementAnimation[baskets.Length];
        InitializeBasketScripts();
        completionTimerScript = gameManager.GetComponent<RecipeCompletionTimer>();
        recipeDisplayScript = gameManager.GetComponent<RecipeDisplayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Reference to VR device that is being used
        var vr = VRDevice.Device;
        //VR device should be connected
        if(vr == null)
            return;
        //Reference to Primary Input controller
        var inputDevice = vr.PrimaryInputDevice;
        //Device should have controllers to play
        if(inputDevice == null)
            return;
        //Returns the RayCast hit result (Similar to Physics.Raycast(..,..,out hit))
        var hitResult = inputDevice.Pointer.CurrentRaycastResult;
        
        //Process button clicks only if the controller is pointed to an object and input is enabled
        if(hitResult.gameObject != null && !disableInput)
        {
            if(inputDevice.GetButtonDown(VRButton.One))
            {
                OnControllerButtonClick(hitResult);
            }
        }
    }

    //Handles the input and actions to be taken when button is clicked
    private void OnControllerButtonClick(UnityEngine.EventSystems.RaycastResult hit)
    {
        if(gameStarted)
        {
            //If ladle is clicked, then execute ladle animation
            if(hit.gameObject.CompareTag("Ladle"))
            {
                hit.gameObject.GetComponent<LadleAnimation>().WhenClicked();
                recordedActions[actionsTaken] = "i";
                actionsTaken++;
            }
            // If pot is clicked, then execute ladle animation
            else if(hit.gameObject.CompareTag("Pot"))
            {
                GameObject.FindWithTag("Ladle").GetComponent<LadleAnimation>().WhenClicked();
                recordedActions[actionsTaken] = "i";
                actionsTaken++;
            }
            // If herb basket is clicked, execute herb movement animation
            else if(hit.gameObject.CompareTag("Herb"))
            {
                hit.gameObject.GetComponent<IngredientMovementAnimation>().StartAnimation();
                recordedActions[actionsTaken] = "h";
                actionsTaken++;
            }
            // If mineral basket is clicked, execute mineral movement animation
            else if(hit.gameObject.CompareTag("Mineral"))
            {
                hit.gameObject.GetComponent<IngredientMovementAnimation>().StartAnimation();
                recordedActions[actionsTaken] = "m";
                actionsTaken++;
            }
            // If mushroom basket is clicked, execute mushroom movement animation
            else if(hit.gameObject.CompareTag("Mushroom"))
            {
                hit.gameObject.GetComponent<IngredientMovementAnimation>().StartAnimation();
                recordedActions[actionsTaken] = "u";
                actionsTaken++;
            }
            // If magic item basket is clicked, execute magic item movement animation
            else if(hit.gameObject.CompareTag("Magic item"))
            {
                hit.gameObject.GetComponent<IngredientMovementAnimation>().StartAnimation();
                recordedActions[actionsTaken] = "g";
                actionsTaken++;
            }

            checkActions();
        }
        else
        {
            // Start the game when user clicks on firewood
            if(hit.gameObject.CompareTag("Fire"))
            {
                //SoundManager.Play("flick");
                hit.gameObject.GetComponent<FireGenerator>().startFire();
                gameStarted = true;
                GameObject.FindWithTag("Pot").GetComponent<ParticleSystem>().Play();
                actionsTaken = 0;
                maxActionsForThisRecipe = 0;
                generateNewRecipe();
            }
        }
    }

    private void InitializeBasketScripts()
    {
        for(int i = 0; i < baskets.Length; i++)
        {
            basketScripts[i] = baskets[i].GetComponent<IngredientMovementAnimation>();
        }
    }

    // Gets new recipe from the level handler and stores the recipe in an array. Also, gets the maximum
    // number of actions that will complete this recipe.
    // Also creates an array to record the actions (ingredients and pot/ladle) taken by the player.
    private void generateNewRecipe()
    {
        levelHandler.recipeGeneratable();
        levelHandler.generateNewRecipe();
        currentRecipe = levelHandler.getCurrentRecipe();
        recipeDisplayScript.DisplayRecipeSprites(currentRecipe);
        maxActionsForThisRecipe = levelHandler.getMaxActions();
        recordedActions = new string[maxActionsForThisRecipe];
        completionTimerScript.StartTimer();
    }

    // After every click on ingredients or pot, check if user has completed specified number of actions.
    // If so, check if they made a correct recipe or not.
    // Show particles and animations according to the outcome.
    private void checkActions()
    {
        // If all the actions are taken, then check all the actions and compare them with the recipe that was generated
        if(actionsTaken == maxActionsForThisRecipe)
        {
            bool success = true;
            // Successful or unsuccessful 
            for(int i = 0; i < currentRecipe.Length; i++)
            {
                if(!Equals(currentRecipe[i], recordedActions[i]))
                {
                    success = false;
                    break;
                }
            }

            // If they match then start the success coroutine. Else start failure coroutine 
            if(success)
            {
                StartCoroutine(successfulRecipe());
            }
            else
            {
                StartCoroutine(failedRecipe());
            }
            actionsTaken = 0;
        }
        else
            return;
    }

    //Disable or enable input from the controller
    void DisableInput(bool value)
    {
        disableInput = value;
    }

    // If true, then successful/failed recipe coroutines won't run until this value changes to false
    void DisableRecipeCoroutine(bool value)
    {
        disableRecipeCoroutine = value;
    }

    // Start the coroutine when user succeeds to create the recipe
    public void StartSuccessfulRecipeCoroutine()
    {
        StartCoroutine(successfulRecipe());
    }

    // Start the coroutine when user fails to create the recipe
    public void StartFailedRecipeCoroutine()
    {
        StartCoroutine(failedRecipe());
    }

    // If recipe is successful then increment the score, show the response through particle system and 
    // generate new recipe. 
    IEnumerator successfulRecipe()
    {
        // Don't start this coroutine until all the ongoing animations are not complete 
        while(disableRecipeCoroutine)
            yield return new WaitForSeconds(0.1f);
        
        // Reset the completion timer
        completionTimerScript.StopTimer();
        //Disable input from VR remote
        DisableInput(true);
        // Run the correct recipe response coroutine
        levelHandler.correctRecipe();
        yield return new WaitForSeconds(0.5f);
        // Play success particles
        particles.PlaySuccessParticles();
        yield return new WaitForSeconds(1.0f);
        // Reset location of all the ingredients
        ResetAllLocations();
        // generate new recipe and show on board
        generateNewRecipe();
        // Re-enable the VR remote inputs 
        DisableInput(false);
    }

    // If recipe is unsuccessful then update the score system, show the response through particle system and 
    // generate new recipe.
    IEnumerator failedRecipe()
    {
        // Don't start this coroutine until all the ongoing animations are not complete 
        while(disableRecipeCoroutine)
            yield return new WaitForSeconds(0.1f);
        
        // Reset the completion timer
        completionTimerScript.ResetTimer();
        //Disable input from VR remote
        DisableInput(true);
        // Run the wrong recipe response coroutine
        levelHandler.wrongRecipe();
        yield return new WaitForSeconds(0.5f);
        // Play failure particles
        particles.PlayFailureParticles();
        yield return new WaitForSeconds(1.0f);
        // Reset location of all the ingredients
        ResetAllLocations();
        // Generate new recipe and show on board
        generateNewRecipe();
        // Re-enable the VR remote inputs 
        DisableInput(false);
    }

    // Reset location of all ingredients to their respective baskets
    private void ResetAllLocations()
    {
        for(int i = 0; i < basketScripts.Length; i++)
        {
            basketScripts[i].ResetIngredientLocation();
        }
    }
}

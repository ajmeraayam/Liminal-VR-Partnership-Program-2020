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
    bool gameStarted = false;
    private GameObject gameManager;
    private LevelHandler levelHandler;
    private GameObject success_fail_particleSystem;
    private RecipeResultParticles particles;
    string[] currentRecipe;
    int actionsTaken;
    int maxActionsForThisRecipe;
    string[] recordedActions;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager");
        success_fail_particleSystem = GameObject.Find("Success Fail PS");
        levelHandler = gameManager.GetComponent<LevelHandler>();
        particles = success_fail_particleSystem.GetComponent<RecipeResultParticles>();
        actionsTaken = 0;
        maxActionsForThisRecipe = 0;
        recordedActions = new string[0];
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

    void generateNewRecipe()
    {
        levelHandler.generateNewRecipe();
        currentRecipe = levelHandler.getCurrentRecipe();
        maxActionsForThisRecipe = levelHandler.getMaxActions();
        recordedActions = new string[maxActionsForThisRecipe];
        // Add calls to recipe board (For display purposes)
    }


    void checkActions()
    {
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

            if(success)
            {
                levelHandler.correctRecipe();
                particles.PlaySuccessParticles();
            }
            else
            {
                levelHandler.wrongRecipe();
                particles.PlayFailureParticles();
            }
        }
        else
            return;
    }

    //Disable or enable input from the controller
    void DisableInput(bool value)
    {
        disableInput = value;
    }

}

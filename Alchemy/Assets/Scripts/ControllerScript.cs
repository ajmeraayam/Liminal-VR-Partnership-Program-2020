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
    // Start is called before the first frame update
    void Start()
    {
        
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
                //SoundManager.Play("stir");
                hit.gameObject.GetComponent<LadleAnimation>().WhenClicked();
            }
            // If pot is clicked, then execute ladle animation
            else if(hit.gameObject.CompareTag("Pot"))
            {
                //SoundManager.Play("stir");
                GameObject.FindWithTag("Ladle").GetComponent<LadleAnimation>().WhenClicked();
            }
            // If herb basket is clicked, execute herb movement animation
            else if(hit.gameObject.CompareTag("Herb"))
            {
                //SoundManager.Play("herb pickup");
                hit.gameObject.GetComponent<IngredientMovementAnimation>().StartAnimation();
            }
            // If mineral basket is clicked, execute mineral movement animation
            else if(hit.gameObject.CompareTag("Mineral"))
            {
                //SoundManager.Play("mineral pickup");
                hit.gameObject.GetComponent<IngredientMovementAnimation>().StartAnimation();
            }
            // If mushroom basket is clicked, execute mushroom movement animation
            else if(hit.gameObject.CompareTag("Mushroom"))
            {
                //SoundManager.Play("mushroom pickup");
                hit.gameObject.GetComponent<IngredientMovementAnimation>().StartAnimation();
            }
            // If magic item basket is clicked, execute magic item movement animation
            else if(hit.gameObject.CompareTag("Magic item"))
            {
                //SoundManager.Play("magic pickup");
                hit.gameObject.GetComponent<IngredientMovementAnimation>().StartAnimation();
            }   
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
            }
        }
    }

    //Disable or enable input from the controller
    void DisableInput(bool value)
    {
        disableInput = value;
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
<<<<<<< Updated upstream
//using UnityEngine.EventSystems;
//using Liminal.SDK.VR.EventSystems;


//IPointerClickHandler to detect pointer click
public class ControllerScript : MonoBehaviour
{
=======

/*Performance improvements - Cache methods for gameobjects (probably in hashmap) that are used often in the game. It will save the time to search for script in the gameobject. 
Another way is to check if OnPointerClick, OnPointerDown and OnPointerUp does the job better or not
*/
public class ControllerScript : MonoBehaviour
{
    //To disable input if needed
    bool disableInput = false;
>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
<<<<<<< Updated upstream
    /*void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10f;
        Ray myray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(transform.position, forward, Color.red);

        if(Physics.Raycast(myray, out hit, 100f))
        {    
            if(hit.collider != null )
            {
                //hit.collider.gameObject.GetComponent<LadleAnimation>().invokeAnimation();
                print(hit.collider.tag);
            }
        }
    }*/
    void Update()
    {
        var vrDevice = VRDevice.Device;

        if(vrDevice == null)
            return;

        var hitResult = vrDevice.PrimaryInputDevice.Pointer.CurrentRaycastResult;
        if(hitResult.gameObject.CompareTag("Ladle"))
        {
            print("Ladle");
        }

    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        var raycast_result = eventData.pointerCurrentRaycast;
        print(raycast_result.gameObject.tag);
    }*/
=======
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

    //Handles the input and actions to taken when button is clicked
    private void OnControllerButtonClick(UnityEngine.EventSystems.RaycastResult hit)
    {
        //If ladle is clicked, then execute ladle animation
        if(hit.gameObject.CompareTag("Ladle"))
        {
            hit.gameObject.GetComponent<LadleAnimation>().WhenClicked();
        }
        else if(hit.gameObject.CompareTag("Fire"))
        {
            
        }
        else
        {

        }
    }

    //Disable or enable input from the controller
    void DisableInput(bool value)
    {
        disableInput = value;
    }

>>>>>>> Stashed changes
}

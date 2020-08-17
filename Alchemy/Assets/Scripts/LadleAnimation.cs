﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadleAnimation : MonoBehaviour
{
    private float timeCounter = 0f;
    private float radius = 0.30f;
    private bool clickBlocked = false;
    private Vector3 startPosition;
    private ControllerScript controllerScript;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        //Cache the controller script to access it quickly
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
    }

    public void WhenClicked()
    {
        //Send message to disable inputs from the controller when ladle is moving
        controllerScript.SendMessage("DisableInput", true);
        float yPos = transform.localPosition.y - 0.3f;
        //Move the ladle down in Y-axis
        transform.localPosition = new Vector3(transform.localPosition.x, yPos, transform.localPosition.z);
        //Start the coroutine to move the ladle in circular motion
        StartCoroutine(invokeAnimation());
    }
    
    //Invokes ladle animation for 6 seconds
    IEnumerator invokeAnimation()
    {
        float timePassed = 0f;
        //For 6 seconds, keep calling moveLadle method
        while(timePassed < 6f)
        {
            Invoke("moveLadle", 0.0001f);
            timePassed += Time.deltaTime;
            yield return null;
        }
        //Reset the position of the ladle
        transform.localPosition = startPosition;
        //After 6 seconds, unblock the input so user can continue playing  
        controllerScript.SendMessage("DisableInput", false);
    }

    //Generate the circular motion using cos and sin values.
    private void moveLadle()
    {
        //Speed of ladle is set to PI, so the ladle will complete a circle in 2 seconds
        timeCounter -= Time.deltaTime * Mathf.PI;
        //Move the ladle in x and z axis, in circular motion
        float x = Mathf.Cos (timeCounter) * radius;
        float z = Mathf.Sin (timeCounter) * radius;
        float y = transform.localPosition.y;
        transform.localPosition = new Vector3 (x, y, z);
    }
}

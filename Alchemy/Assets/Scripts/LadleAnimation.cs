using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadleAnimation : MonoBehaviour
{
    private float timeCounter = 0f;
    private float radius = 0.30f;
    private bool clickBlocked = false;

    // Update is called once per frame
    void Update()
    {
        // When left mouse button is clicked and clicking is unblocked
        if(Input.GetMouseButtonDown(0) && !clickBlocked)
        {
            //Move the ladle down in Y-axis
            transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
            //Start the coroutine to move the ladle in circular motion
            StartCoroutine(invokeAnimation());
        }
    }

    //Invokes ladle animation for 6 seconds
    IEnumerator invokeAnimation()
    {
        //During the time of animation, mouse clicking is blocked
        clickBlocked = true;
        float timePassed = 0f;
        //For 6 seconds, keep calling moveLadle method
        while(timePassed < 6f)
        {
            Invoke("moveLadle", 0.0001f);
            timePassed += Time.deltaTime;
            yield return null;
        }
        //After 6 seconds, unblock the clicking so user can animate ladle again 
        clickBlocked = false;
        //Reset the position of the ladle
        transform.position = new Vector3(0.3f, 0.6f, 0f);
    }

    //Generate the circular motion using cos and sin values.
    private void moveLadle()
    {
        //Speed of ladle is set to PI, so the ladle will complete a circle in 2 seconds
        timeCounter -= Time.deltaTime * Mathf.PI;
        //Move the ladle in x and z axis, in circular motion
        float x = Mathf.Cos (timeCounter) * radius;
        float z = Mathf.Sin (timeCounter) * radius;
        float y = transform.position.y;
        transform.position = new Vector3 (x, y, z);
    }
}

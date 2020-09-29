using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allows the scene to be updated while editing outside of Play Mode.
//We use it to show changes in the sprites and text as you make them.

[ExecuteInEditMode]
public class UpdateInEditMode : MonoBehaviour 
{
	
    Timer timer;

    void OnEnable()
    {
        timer = this.GetComponent<Timer>();
    }

	// Update is called once per frame
	void Update () 
    {
        timer.UpdateEditorStuff();
	}
}

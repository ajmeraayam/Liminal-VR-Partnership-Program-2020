using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

//[CustomEditor(typeof(Timer))]
//[CanEditMultipleObjects]
public class TimerEditor : MonoBehaviour
{
    //this texture is the banner image at top of script in inspector.
    

    public void OnInspectorGUI () 
    {
        

        // DrawDefaultInspector();

        //The Horizontals and FlexibleSpaces center our elements in the inspector.
        /*
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        */
        //The following creates the 3 buttons in the inspector. The foreach enables functionality for multi-object selecting.
        /*
        if (GUILayout.Button("Start Timer", GUILayout.Width(200), GUILayout.Height(23)))
        {
            foreach (var timer in targets.Cast<Timer>())
            {             
            timer.StartTimer();
            }
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Pause Timer", GUILayout.Width(200), GUILayout.Height(23)))
        {
            foreach (var timer in targets.Cast<Timer>())
            {
                timer.PauseTimer();
            }
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Reset Timer", GUILayout.Width(200), GUILayout.Height(23)))
        {
                foreach (var timer in targets.Cast<Timer>())
                {
                    timer.ResetTimer();
                }
        }  

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        */
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // UI elements
    public Text streak;
    public Text totalScore;
    public Text currentLevel;
    public Text actionsLeft;
    private LevelHandler levelHandlerScript;
    private ControllerScript controllerScript;

    void Start()
    {
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        levelHandlerScript = GetComponent<LevelHandler>();
        totalScore.text = "Total: " + 0;
        streak.text = "Total: " + 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScores()
    {
        int total = levelHandlerScript.Score;
        int current = levelHandlerScript.CorrectStreak;
        totalScore.text = "Total: " + total;
        streak.text = "Current: " + current;
        if (controllerScript.TutorialComplete)
        {
            currentLevel.text = "Current Level: " + levelHandlerScript.CurrentRecipeLevel;
            int a_left = controllerScript.MaxActionsForthisRecipe - controllerScript.ActionsTaken;
            actionsLeft.text = "Recipe Actions Left: " + a_left;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // UI elements
    public Text streak;
    public Text totalScore;
    private LevelHandler levelHandlerScript;

    void Start()
    {
        levelHandlerScript = GetComponent<LevelHandler>();
        totalScore.text = "Total: " + 0;
        streak.text = "Total: " + 0;
    }

    // Update is called once per frame
    void Update()
    {
        int total = levelHandlerScript.Score;
        int current = levelHandlerScript.CorrectStreak;
        totalScore.text = "Total: " + total;
        streak.text = "Current: " + current;
    }
}

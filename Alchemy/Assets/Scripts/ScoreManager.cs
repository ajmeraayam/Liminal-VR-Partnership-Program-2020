using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update

    // scores init
    public static int current = 0;
    public static int total = 0;

    // UI elements
    public Text currentScore;
    public Text totalScore;

    void Start()
    {
        totalScore.text = "Total: " + 0;
        currentScore.text = "Total: " + 0;
    }

    // Update is called once per frame
    void Update()
    {
        totalScore.text = "Total: " + total;
        currentScore.text = "Current: " + current;
    }

    public static void success()
    {
        // add a counter for each successful brews
        current += 1;
        total += 1;

        
    }

    public static void fail()
    {
        // reset current counter to 0
        current = 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private string[] recipe = {"h", "u", "i"};
    private RecipeDisplayManager displayManagerScript;
    private ControllerScript controllerScript;
    private Text instructionText;
    private int nextAction;
    private Coroutine tutorialCoroutine = null;
    private bool complete;

    void Awake()
    {
        instructionText = GameObject.Find("Instruction Text").GetComponent<Text>();
        displayManagerScript = GetComponent<RecipeDisplayManager>();
        controllerScript = GameObject.Find("VRAvatar").GetComponent<ControllerScript>();
        nextAction = 0;
        complete = false;
    }

    public void StartTutorial()
    {
        tutorialCoroutine = StartCoroutine(TutorialCoroutine());
    }

    private IEnumerator TutorialCoroutine()
    {
        controllerScript.SendMessage("DisableInput", true);
        instructionText.text = "Welcome to Alchemy!";
        yield return new WaitForSeconds(4f);
        instructionText.text = "The recipes will show up on the other board.";
        yield return new WaitForSeconds(4f);
        instructionText.text = "The colours on the board correspond to the labels on the basket";
        yield return new WaitForSeconds(4f);
        // Display the tutorial recipe on the board
        DisplayRecipe();
        nextAction = 0;
        instructionText.text = "You have to make potions by following the recipes. Click on the shining basket";
        // Shine a spotlight
        controllerScript.SendMessage("DisableInput", false);
        // Wait for player to complete the actions
        while(true)
        {
            yield return new WaitForEndOfFrame();
            // When all the actions are taken, we exit this loop
            if(complete)
                break; 
        }

        while(controllerScript.IsRecipeCoroutineDisabled())
        {
            yield return new WaitForEndOfFrame();
        }

        instructionText.text = "Tutorial Completed! Good Luck!";
        yield return new WaitForSeconds(5f);
        controllerScript.TutorialComplete = true;
        instructionText.text = "";
        controllerScript.StartGame();
    }

    private void DisplayRecipe()
    {
        displayManagerScript.DisplayRecipeSprites(recipe);
    }

    public void RecordAction(string action)
    {
        if(recipe[nextAction].Equals(action))
        {
            nextAction++;
            // If some actions left in the tutorial
            if(nextAction < recipe.Length)
            {
                instructionText.text = "Great! Now click on the next shining object";
                // Shine a spotlight
            }
            // If this was the last action of the tutorial
            else
            {
                complete = true;
            }
        }
        else
        {
            instructionText.text = "Wrong basket. Click on the shining object";
        }
    }

    public void SkipTutorial()
    {
        StartCoroutine(SkipTute());
    }

    private IEnumerator SkipTute()
    {
        controllerScript.DisableSkipTutorialButton();
        while(controllerScript.IsRecipeCoroutineDisabled())
        {
            yield return new WaitForEndOfFrame();
        }

        StopCoroutine(tutorialCoroutine);
        controllerScript.SendMessage("DisableInput", true);
        // Destroy recipe from display
        displayManagerScript.DisappearRecipe();
        instructionText.text = "Tutorial Skipped! Good Luck!";
        yield return new WaitForSeconds(3f);
        controllerScript.TutorialComplete = true;
        instructionText.text = "";
        controllerScript.StartGame();
    }
}

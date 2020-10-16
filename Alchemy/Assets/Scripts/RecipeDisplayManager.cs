using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDisplayManager : MonoBehaviour
{
    public GameObject herb;
    public GameObject mushroom;
    public GameObject mineral;
    public GameObject magicItem;
    public GameObject arrow;
    public GameObject mix;
    public GameObject recipeBoard;
    private Transform recipeBoardLayout;
    private RectTransform recipeBoardRectTransform;
    private float boardWidth;
    private float displayableWidth;
    private GameObject[] actionObjects;
    private float yPosForSprites;

    void Awake()
    {
        recipeBoardLayout = recipeBoard.transform.Find("Layout");
        recipeBoardRectTransform = recipeBoard.GetComponent<RectTransform>();
        boardWidth = recipeBoardRectTransform.rect.width;
        displayableWidth = boardWidth - 35f;
        yPosForSprites = 12f;
    }

    public void DisplayRecipeSprites(string[] recipe)
    {
        if(actionObjects != null)
            DestroyOldGameObjects();
        // Number of actions in the recipe
        int size = recipe.Length;
        // Total icons to be displayed
        int totalIcons = (size * 2) - 1;
        // Index of the action that should be at the center of the board
        int mid = (totalIcons - 1) / 2;
        // Distance between each sprite (Center of one sprite to center of another)
        float separation = displayableWidth / totalIcons;
        // Array of GameObjects (Sprites) generated to display the recipe ingredients
        actionObjects = new GameObject[totalIcons];
        
        for(int i = 0; i < totalIcons; i = i + 2)
        {
            // Sprite for any of the action
            GameObject actionObject;
            // Instantiating sprites according to the action.
            // These sprites are child object of the layout of the GUI component
            if(Equals(recipe[i / 2], "h"))
            {
                actionObject = Instantiate(herb, recipeBoardLayout, false) as GameObject;
            }
            else if(Equals(recipe[i / 2], "u"))
            {
                actionObject = Instantiate(mushroom, recipeBoardLayout, false) as GameObject;
            }
            else if(Equals(recipe[i / 2], "m"))
            {
                actionObject = Instantiate(mineral, recipeBoardLayout, false) as GameObject;
            }
            else if(Equals(recipe[i / 2], "g"))
            {
                actionObject = Instantiate(magicItem, recipeBoardLayout, false) as GameObject;
            }
            else
            {
                actionObject = Instantiate(mix, recipeBoardLayout, false) as GameObject;
            }

            actionObjects[i] = actionObject;
            // There is always 1 arrow less than the number of actions (Since after the last action there is no connector)
            if((i + 1) < totalIcons)
            {
                // Connector sprite between 2 recipe action sprites
                GameObject arrowSprite = Instantiate(arrow, recipeBoardLayout, false);
                actionObjects[i + 1] = arrowSprite;
            }
        }
        
        // Setting the sprite that should be at the middle to (0, 0, 0).
        // It will place the sprite in the middle of the board
        actionObjects[mid].transform.localPosition = new Vector3(0f, yPosForSprites, 0f);
        // Sprites stored in the array before the mid index should be on the left side of the board
        for(int i = mid - 1; i >= 0; i--)
        {
            float x_loc = 0f - ((mid - i) * separation);
            actionObjects[i].transform.localPosition = new Vector3(x_loc, yPosForSprites, 0f);
        }
        // Sprites stored in the array after the mid index should be on the right side of the board
        for(int i = mid + 1; i < totalIcons; i++)
        {
            float x_loc = 0f + ((i - mid) * separation);
            actionObjects[i].transform.localPosition = new Vector3(x_loc, yPosForSprites, 0f);
        }
    }

    private void DestroyOldGameObjects()
    {
        // Destroy all the older game objects from the layout.
        for(int i = 0; i < actionObjects.Length; i++)
        {
            Destroy(actionObjects[i]);
        }
    }

    public void DisappearRecipe()
    {
        if(actionObjects != null)
            DestroyOldGameObjects();
    }
}

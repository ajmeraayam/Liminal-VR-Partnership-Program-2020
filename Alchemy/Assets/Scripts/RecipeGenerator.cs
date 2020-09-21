using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeGenerator : MonoBehaviour
{
    private TextAsset txtFile;
    //save all recipes,string[] means each item in recipe, recipeCache means recipe
    private List<string[]> recipeCache = new List<string[]>();
    private int totalLevels = 5;
    void Start()
    {
        // Get all the recipes first
        ExtractAll();
        //For example (recipeCache[0], 0) means level1, first recipe
        ExtractRecipe(recipeCache[0], 0);
        //for example, get random recipe in level 4
        GetRandomRecipe(3);
    }

    //Get all the recipes
    private void ExtractAll()
    {
        for(int i = 0; i < totalLevels; i++)
        {
            //Loop five times, get a recipe from Level 1 - 5
            txtFile = (TextAsset)(Resources.Load("level" + (i + 1)));
            // Read the contents of the .txt file in a string 
            string fileContents = txtFile.text;
            //save each recipes, cut by ","
            recipeCache.Add(fileContents.Split(','));
        }
    }
    //Get the recipes, k means the recipe 
    private string[] ExtractRecipe(string[] levelRecipeArray, int k) 
    {
        if(k >= levelRecipeArray.Length)
        {
            return new string[0];
        }
        else
        {
            string[] recipe = levelRecipeArray[k].Split('+');
            for(int i = 0; i < recipe.Length; i++)
            {
                if(recipe[i].IndexOf("herb", StringComparison.OrdinalIgnoreCase) >= 0)
                    recipe[i] = "h";
                
                else if(recipe[i].IndexOf("mushroom", StringComparison.OrdinalIgnoreCase) >= 0)
                    recipe[i] = "u";
                
                else if(recipe[i].IndexOf("mineral", StringComparison.OrdinalIgnoreCase) >= 0)
                    recipe[i] = "m";
                
                else if(recipe[i].IndexOf("magic", StringComparison.OrdinalIgnoreCase) >= 0)
                    recipe[i] = "g";
                
                else if(recipe[i].IndexOf("mix", StringComparison.OrdinalIgnoreCase) >= 0)
                    recipe[i] = "i";
            }
            return recipe;
        }
        
    }

    public string[] GetRandomRecipe(int level) 
    {
        // Generate a recipe index. Random index should be between 0 and number of recipes - 1
        int numRecipe = UnityEngine.Random.Range(0, recipeCache[level].Length - 1);
        // Return the recipe at given index
        return ExtractRecipe(recipeCache[level], numRecipe);
    }

}

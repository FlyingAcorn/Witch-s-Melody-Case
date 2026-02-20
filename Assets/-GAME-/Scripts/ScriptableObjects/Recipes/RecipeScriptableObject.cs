using System.Collections.Generic;
using _GAME_.Scripts.FoodRelated;
using _GAME_.Scripts.FoodRelated.RecipeObject;
using UnityEngine;

namespace _GAME_.Scripts.ScriptableObjects.Recipes
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/RecipeScriptableObject")]
    public class RecipeScriptableObject : ScriptableObject
    {
        
        public RecipeObject.RecipeObjects recipeObject;
        public bool hasNoRecipe; // maybe make a editor script to hide elements when this is true
        public List<Food.FoodList> mainIngredients;
        public List<Food.FoodList> fillings;
        public List<Food.FoodList> sauces;
    }
}


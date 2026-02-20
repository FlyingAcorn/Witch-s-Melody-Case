using System.Collections.Generic;
using _GAME_.Scripts.FoodRelated;
using UnityEngine;

namespace _GAME_.Scripts.ScriptableObjects.Recipes
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/RecipeScriptableObject")]
    public class RecipeScriptableObject : ScriptableObject
    {
        public Food.FoodList coreFood;
        public List<Food.FoodList> mainIngredients;
        public List<Food.FoodList> fillings;
        public List<Food.FoodList> sauces;
    }
}

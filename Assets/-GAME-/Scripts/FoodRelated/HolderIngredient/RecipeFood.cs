using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts.FoodRelated.HolderIngredient
{
    public abstract class RecipeFood : Food
    {
        [SerializeField] protected List<FoodList> foodsInside;
        [Header("RecipeConfigurations")] 
        [SerializeField] protected List<FoodList> allowedMainIngredients;
        [SerializeField] protected List<FoodList> allowedFillings;
        [SerializeField] protected List<FoodList> allowedSauces; // sauces will cast a ray to check
         private bool _mainIngredientSelected;
         [NonSerialized] public bool OnCuttingBoard;
         //TODO: arrange protected private  keys of scripts
        protected override void Awake()
        {
            base.Awake();
        }
        public override void Interact()
        {
        }
        // recipefood + cookable obj (check if cooked)+ 2filling(max)+2 sauce (max)

        private void OnTriggerEnter(Collider other)
        {
            if (!OnCuttingBoard) return;
            if (other.TryGetComponent(out Food food) && CheckFood(food))
            {
                
                if (allowedMainIngredients.Contains(food.foodType)) _mainIngredientSelected  = true;
                foodsInside.Add(food.foodType);
            }
        }

        private bool CheckFood(Food food)
        {
            if (foodsInside.Contains(food.foodType)) return false;
            if (!allowedMainIngredients.Contains(food.foodType) && !allowedFillings.Contains(food.foodType)) return false;
            if (allowedMainIngredients.Contains(food.foodType) && _mainIngredientSelected) return false;
            if (food.TryGetComponent(out CookableObj _) && !food.IsCooked)  return false;
                return true;
        }
        //TODO: sauces will have their own separate method
        //TODO: DO movement of the pieces
    }
}

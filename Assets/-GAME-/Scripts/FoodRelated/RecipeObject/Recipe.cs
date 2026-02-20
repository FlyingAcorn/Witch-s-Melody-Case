using System;
using System.Collections.Generic;
using System.Linq;
using _GAME_.Scripts.FoodRelated.CookedFood;
using DG.Tweening;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated.RecipeObject
{
    public class Recipe : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform centerPos;
        [NonSerialized] public RecipeObject MyObject;
        [SerializeField] private List<FoodMeshInfo> meshReferences;
        [Header("RecipeConfigurations")] 
        [SerializeField] private List<Food.FoodList> allowedMainIngredients;
        [SerializeField] private List<Food.FoodList> allowedFillings;
        [SerializeField] private List<Food.FoodList> allowedSauces; // sauces will cast a ray to check
        private bool _mainIngredientSelected;
        public List<Food.FoodList> foodsInside;
        
         private void Awake()
         {
             MyObject  = GetComponent<RecipeObject>();
         }

         private void OnTriggerStay(Collider other)
        {
            if (!MyObject.OnAHolder) return;
            if (other.TryGetComponent(out Food food) && CheckFood(food) && !food.IsPickedUp)
            {
                if (allowedMainIngredients.Contains(food.foodType)) _mainIngredientSelected  = true;
                foodsInside.Add(food.foodType);
                MoveFood(food);
            }
        }
        private bool CheckFood(Food food)
        {
            if (food.TryGetComponent(out CookableObj _) && !food.PrepDone)  return false;
            if (foodsInside.Contains(food.foodType)) return false;
            if (allowedMainIngredients.Contains(food.foodType) && !_mainIngredientSelected)
            {
                return true;
            }
            if (allowedFillings.Contains(food.foodType))
            {
                return true;
            }
            if (allowedSauces.Contains(food.foodType))
            {
                return true;
            }
            return false;
        }

        private void MoveFood(Food food)
        {
            food.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            food.GetComponent<Collider>().enabled = false;
            food.transform.DOMove(centerPos.position, 0.1f).OnComplete(() =>
            {
                food.gameObject.SetActive(false);
                foreach (var meshInfo in meshReferences.Where(meshInfo => meshInfo.myType == food.foodType))
                {
                    meshInfo.gameObject.SetActive(true);
                }
            });
        }

       
        //TODO: sauces will have their own separate method
    }
}

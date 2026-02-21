using System.Collections.Generic;
using System.Linq;
using _GAME_.Scripts.FoodRelated;
using _GAME_.Scripts.FoodRelated.RecipeObject;
using _GAME_.Scripts.ScriptableObjects.Recipes;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME_.Scripts.Customer
{
    public class CustomerEndless : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform startPosition;
        [SerializeField] private Transform buyPosition;
        [SerializeField] private Transform endPosition;
        [Header("CustomerSettings")] 
        [SerializeField] private List<CustomerTypeInfo> customerTypes;
        [SerializeField] private List<RecipeScriptableObject> knownRecipes;
        [SerializeField] private int moveTime;
        private CustomerTypeInfo _currentCustomerType;
        [SerializeField] private RecipeObject.RecipeObjects selectedRecipeObject;
        [SerializeField] private List<Food.FoodList> selectedIngredients;


        private enum CustomerState
        {
            Starting,
            Buying,
            Ending,
        }
        
        private CustomerState _currentState;

        private void Start()
        {
            UpdateCustomerState(CustomerState.Starting);
        }

        private void UpdateCustomerState(CustomerState state)
        {
            _currentState = state;

            if (state == CustomerState.Starting)
            {
                SelectCustomerType();
                MoveTo(buyPosition);
                SelectRecipe();
            }

            if (state == CustomerState.Buying)
            {
                MoveTo(endPosition);
                selectedIngredients.Clear();
                selectedIngredients.TrimExcess();
            }

            if (state == CustomerState.Ending)
            {
                transform.position = startPosition.position;
                UpdateCustomerState(CustomerState.Starting);
            }
        }

        private void MoveTo(Transform target)
        {
            transform.DOMove(target.position, moveTime).OnComplete(() =>
            {
                if(_currentState == CustomerState.Buying) UpdateCustomerState(CustomerState.Ending);
            }) ;
        }

        private void SelectCustomerType()
        {
            if(_currentCustomerType != null)_currentCustomerType.gameObject.SetActive(false);
            var selectedType = customerTypes[Random.Range(0, customerTypes.Count)];
            _currentCustomerType = selectedType;
            selectedType.gameObject.SetActive(true);
        }

        private void SelectRecipe()
        {
            var selectedRecipe = knownRecipes[Random.Range(0, knownRecipes.Count)];
            selectedRecipeObject = selectedRecipe.recipeObject;
            var ingredients = new List<Food.FoodList>();
            ingredients.AddRange(RandomSelection(selectedRecipe.mainIngredients,1));
            ingredients.AddRange(RandomSelection(selectedRecipe.fillings));
            ingredients.AddRange(RandomSelection(selectedRecipe.sauces));
            var foodLists = ingredients.Where(i => ingredients.Count(j => i == j) == 1).ToList();
            foodLists.TrimExcess();
            selectedIngredients = foodLists;
        }

        private bool CompareRecipes(Recipe food)
        {
            if (selectedRecipeObject != food.MyObject.recipeObjectsType) return false;
            if (!selectedIngredients.All(food.foodsInside.Contains)
                || selectedIngredients.Count != food.foodsInside.Count) return false;
            return true;
        }

        private List<Food.FoodList> RandomSelection(List<Food.FoodList> list, int repeat =0)
        {
            var timesOfRepeat = repeat == 0 ? Random.Range(0, list.Count)+1 : repeat;
            var copyList = new List<Food.FoodList>(list);
            if (copyList.Count == 0) return copyList;
            var resultList = new List<Food.FoodList>();
            for (var i = 0; i < timesOfRepeat; i++)
            {
                var selectedFood = copyList[Random.Range(0, copyList.Count)];
                resultList.Add(selectedFood);
                copyList.Remove(selectedFood);
            }
            return resultList;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Recipe recipeFood) && CompareRecipes(recipeFood))
            {
                other.gameObject.SetActive(false);
                UpdateCustomerState(CustomerState.Buying);
            }
            else
            {
                // destroy object
            }
        }
    }
}

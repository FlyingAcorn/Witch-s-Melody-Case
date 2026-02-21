using System.Collections;
using System.Collections.Generic;
using _GAME_.Scripts.FoodRelated.MachineScripts;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated.GrilledFood
{
    public class GrillableObject : MonoBehaviour
    {
        private enum FoodState
        {
            Raw,
            Cooked,
            Burned
        }
        [SerializeField] private Renderer myRenderer;
        [Header("FoodConfigurations")] 
        [SerializeField] private int cookTime;
        [SerializeField] private int burnTime;
        [SerializeField] private FoodState currentFoodState;
        [SerializeField] private List<Material> foodStateMaterials;
        private Coroutine _currentCoroutine;
        private Food _food;
        
        private void Awake()
        {
            _food = GetComponent<Food>();
            UpdateFoodState(FoodState.Raw);
        }

        private void UpdateFoodState(FoodState state)
        {
            currentFoodState = state;
            if (currentFoodState == FoodState.Raw)
            {
                _food.PrepDone = false;
                _food.objectInteractMessage = nameof(FoodState.Raw)+gameObject.name;
            }
            if (currentFoodState == FoodState.Cooked)
            {
                _food.objectInteractMessage = nameof(FoodState.Cooked)+gameObject.name;
                _food.PrepDone = true;
            }

            if (currentFoodState == FoodState.Burned)
            {
                _food.objectInteractMessage = nameof(FoodState.Burned)+gameObject.name;
                _food.PrepDone = false;
            }
            myRenderer.material = foodStateMaterials[(int)state];
        }

        private IEnumerator Cooking()
        {
            while (currentFoodState == FoodState.Raw)
            {
                yield return new WaitForSeconds(cookTime);
                UpdateFoodState(FoodState.Cooked);
                
            }

            while (currentFoodState == FoodState.Cooked)
            {
                
                yield return new WaitForSeconds(burnTime); 
                UpdateFoodState(FoodState.Burned);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Grill _))
            {
                _currentCoroutine = StartCoroutine(Cooking());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Grill _)) return;
            if (_currentCoroutine == null) return;
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }
}

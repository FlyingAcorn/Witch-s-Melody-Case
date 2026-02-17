using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated
{
    public class CookedObj : MonoBehaviour,IInteractable
    {
        private enum FoodState
        {
            Raw,
            Cooked,
            Burned
        }
        [SerializeField] private Renderer myRenderer;
        [Header("ObjectConfigurations")] 
        [SerializeField] private string objectInteractMessage;
        [SerializeField] private bool isInteractable;
        [SerializeField] private bool canBePickedUp;
        [SerializeField] private bool isSharp;
        [Header("FoodConfigurations")] 
        [SerializeField] private int cookTime;
        [SerializeField] private FoodState currentFoodState;
        [SerializeField] private List<Material> foodStateMaterials;
        private Coroutine _currentCoroutine;
        
        public string InteractMessage => objectInteractMessage;
        public GameObject InteractObject => gameObject;
        public bool IsInteractable => isInteractable;
        public bool CanBePickedUp => canBePickedUp;
        public bool IsSharp => isSharp;
        public void Interact()
        {
        }

        private void Awake()
        {
            UpdateFoodState(FoodState.Raw);
        }

        private void UpdateFoodState(FoodState state)
        {
            currentFoodState = state;
            if (currentFoodState == FoodState.Raw)
            {
                objectInteractMessage = nameof(FoodState.Raw)+gameObject.name;
                
            }
            if (currentFoodState == FoodState.Cooked)
            {
                objectInteractMessage = nameof(FoodState.Cooked)+gameObject.name;
            }

            if (currentFoodState == FoodState.Burned)
            {
                objectInteractMessage = nameof(FoodState.Burned)+gameObject.name;
            }
            myRenderer.material = foodStateMaterials[(int)state];
        }

        private IEnumerator Cooking()
        {
            while (currentFoodState != FoodState.Burned)
            {
                yield return new WaitForSeconds(cookTime);
                if (currentFoodState == FoodState.Raw) UpdateFoodState(FoodState.Cooked);
                else if (currentFoodState == FoodState.Cooked) UpdateFoodState(FoodState.Burned);
                
                yield return null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Stove>(out Stove _))
            {
                _currentCoroutine = StartCoroutine(Cooking());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Stove>(out Stove _)) return;
            if (_currentCoroutine == null) return;
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }
}

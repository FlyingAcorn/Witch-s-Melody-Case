using System;
using _GAME_.Scripts.FoodRelated.MachineScripts;
using Unity.VisualScripting;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated.RecipeObject
{
    public class FryBag : RecipeObject
    {
        private Camera _camera;
        private Recipe _recipe;
        [SerializeField] private LayerMask layermask;
        
        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
            _recipe = gameObject.GetComponent<Recipe>();
        }
        public override void Interact()
        {
            if (!Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward)
                    , out RaycastHit hit, 5, layermask)) return;
            if (!hit.transform.TryGetComponent(out Fryer fryer) || fryer.currentState != Fryer.FryerStates.ResultFried) return;
            if (_recipe.CheckFood(fryer.CurrentFriableFood.Food))
            {
                _recipe.MainIngredientSelected = true;
                _recipe.foodsInside.Add(fryer.CurrentFriableFood.Food.foodType);
                fryer.ResetState();
            }
            // change model
        }
    }
}

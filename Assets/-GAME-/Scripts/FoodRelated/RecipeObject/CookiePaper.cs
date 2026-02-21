using _GAME_.Scripts.FoodRelated.MachineScripts;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated.RecipeObject
{
    public class CookiePaper : RecipeObject
    {
        private Camera _camera;
        private Recipe _recipe;
        [SerializeField] private LayerMask layerMask;
        
        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
            _recipe = gameObject.GetComponent<Recipe>();
        }
        public override void Interact()
        {
            if (!Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward)
                    , out RaycastHit hit, 5, layerMask)) return;
            if (!hit.transform.TryGetComponent(out Microwave microwave) || microwave.currentState != Microwave.MicrowaveState.Finished) return;
            if (_recipe.CheckFood(microwave.CurrentMicrowaveFood.Food))
            {
                _recipe.MainIngredientSelected = true;
                _recipe.foodsInside.Add(microwave.CurrentMicrowaveFood.Food.foodType);
                _recipe.ChangeMesh(microwave.CurrentMicrowaveFood.Food);
                microwave.ResetState();
            }
            // change model
        }
    }
}

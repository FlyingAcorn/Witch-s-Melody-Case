using _GAME_.Scripts.FoodRelated.HolderIngredient;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated
{
    public class RecipeHolder : MonoBehaviour
    {
        [SerializeField] private Transform holdPos;
        private RecipeFood _heldObject;

        private void FixedUpdate()
        {
            if (_heldObject == null) return;
            if (!_heldObject.MyFood.IsPickedUp)
            {
                MoveToPos();
            }
        }

        private void MoveToPos()
        {
            if(_heldObject.MyFood.RigidBody.useGravity) _heldObject.MyFood.RigidBody.useGravity = false;
            _heldObject.transform.position = 
                Vector3.MoveTowards(_heldObject.transform.position,holdPos.position,4f * Time.fixedDeltaTime);
            var direction =holdPos.position-_heldObject.transform.position;
            var toRotation = Quaternion.Euler(direction);
            _heldObject.transform.rotation = Quaternion.Lerp(_heldObject.transform.rotation, toRotation,4f * Time.fixedDeltaTime);
        }
        
        private void OnTriggerEnter(Collider obj)
        {
            if (obj.TryGetComponent(out RecipeFood food) && _heldObject == null && !food.MyFood.IsPickedUp)
            {
                food.OnCuttingBoard = true;
                _heldObject = food;
                _heldObject.MyFood.CanBeThrown = false;
                food.MyFood.RigidBody.linearVelocity = Vector3.zero;
            }
        }
        private void OnTriggerExit(Collider obj)
        {
            if (obj.TryGetComponent(out RecipeFood food) && _heldObject == food && food.MyFood.IsPickedUp)
            {
                food.OnCuttingBoard = false;
                _heldObject.MyFood.CanBeThrown = true;
                _heldObject = null;
            }
        }
    }
}

using System;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated
{
    public class CuttingBoard : MonoBehaviour
    {
        [SerializeField] private Transform holdPos;
        private RecipeFood _heldObject;

        private void FixedUpdate()
        {
            if (_heldObject == null) return;
            if (!_heldObject.IsPickedUp)
            {
                MoveToPos();
            }
        }

        private void MoveToPos()
        {
            if(_heldObject.InteractRigidbody.useGravity) _heldObject.InteractRigidbody.useGravity = false;
            _heldObject.transform.position = 
                Vector3.MoveTowards(_heldObject.transform.position,holdPos.position,2f * Time.fixedDeltaTime);
            var direction =holdPos.position-_heldObject.transform.position;
            var toRotation = Quaternion.Euler(direction);
            _heldObject.transform.rotation = Quaternion.Slerp(_heldObject.transform.rotation, toRotation,2f * Time.fixedDeltaTime);
        }
        
        private void OnTriggerEnter(Collider obj)
        {
            if (obj.TryGetComponent(out RecipeFood food) && _heldObject == null)
            {
                food.InteractRigidbody.linearVelocity = Vector3.zero;
                _heldObject = food;
            }
        }
        private void OnTriggerExit(Collider obj)
        {
            if (obj.TryGetComponent(out RecipeFood food) && _heldObject == food )
            {
                if(!_heldObject.IsPickedUp) _heldObject.InteractRigidbody.useGravity = true;
                _heldObject = null;
            }
        }
    }
}

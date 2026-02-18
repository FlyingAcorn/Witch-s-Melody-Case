using System;
using _GAME_.Scripts.FoodRelated.HolderIngredient;
using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts.FoodRelated
{
    public class CuttingBoard : MonoBehaviour
    {
        [SerializeField] private Transform holdPos;
        [SerializeField] private RecipeFood heldObject;

        private void FixedUpdate()
        {
            if (heldObject == null) return;
            if (!heldObject.IsPickedUp)
            {
                MoveToPos();
            }
        }

        private void MoveToPos()
        {
            if(heldObject.RigidBody.useGravity) heldObject.RigidBody.useGravity = false;
            heldObject.transform.position = 
                Vector3.MoveTowards(heldObject.transform.position,holdPos.position,4f * Time.fixedDeltaTime);
            var direction =holdPos.position-heldObject.transform.position;
            var toRotation = Quaternion.Euler(direction);
            heldObject.transform.rotation = Quaternion.Lerp(heldObject.transform.rotation, toRotation,4f * Time.fixedDeltaTime);
        }
        
        private void OnTriggerEnter(Collider obj)
        {
            if (obj.TryGetComponent(out RecipeFood food) && heldObject == null && !food.IsPickedUp)
            {
                food.OnCuttingBoard = true;
                heldObject = food;
                heldObject.CanBeThrown = false;
                food.RigidBody.linearVelocity = Vector3.zero;
            }
        }
        private void OnTriggerExit(Collider obj)
        {
            if (obj.TryGetComponent(out RecipeFood food) && heldObject == food && food.IsPickedUp)
            {
                food.OnCuttingBoard = false;
                heldObject.CanBeThrown = true;
                heldObject = null;
            }
        }
    }
}

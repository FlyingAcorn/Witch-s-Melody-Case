using _GAME_.Scripts.FoodRelated.HolderIngredient;
using DG.Tweening;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated
{
    public class RecipeHolder : MonoBehaviour
    {
        [SerializeField] private Transform holdPos;
        private void MoveToPos(RecipeFood obj)
        {
            obj.transform.DOMove(holdPos.position, 0.1f);
            obj.transform.DORotate(holdPos.eulerAngles, 0.1f);
        }
        
        private void OnTriggerEnter(Collider obj)
        {
            if (obj.TryGetComponent(out RecipeFood food) && !food.MyFood.IsPickedUp)
            {
                food.OnCuttingBoard = true;
               
                food.MyFood.RigidBody.isKinematic = true;
                MoveToPos(food);
            }
        }
        
        private void OnTriggerExit(Collider obj)
        {
            if (obj.TryGetComponent(out RecipeFood food) && food.MyFood.IsPickedUp)
            {
                food.OnCuttingBoard = false;
               
            }
        }
    }
}

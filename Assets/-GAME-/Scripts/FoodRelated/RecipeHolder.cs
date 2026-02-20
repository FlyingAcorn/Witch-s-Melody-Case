using _GAME_.Scripts.FoodRelated.RecipeObject;
using DG.Tweening;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated
{
    public class RecipeHolder : MonoBehaviour
    {
        [SerializeField] private Transform holdPos;
        private void MoveToPos(Recipe obj)
        {
            obj.transform.DOMove(holdPos.position, 0.1f);
            obj.transform.DORotate(holdPos.eulerAngles, 0.1f);
        }
        
        private void OnTriggerEnter(Collider obj)
        {
            if (obj.TryGetComponent(out Recipe food) && !food.MyObject.IsPickedUp)
            {
                food.OnHolder = true;
                food.MyObject.RigidBody.isKinematic = true;
                MoveToPos(food);
            }
        }
        
        private void OnTriggerExit(Collider obj)
        {
            if (obj.TryGetComponent(out Recipe food) && food.MyObject.IsPickedUp)
            {
                food.OnHolder = false;
            }
        }
    }
}

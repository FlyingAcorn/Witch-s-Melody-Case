using System.Collections;
using _GAME_.Scripts.FoodRelated.RecipeObject;
using DG.Tweening;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated.MachineScripts
{
    public class Microwave : Interactable
    {
        [Header("References")] 
        [SerializeField] private Transform microwaveTransform;
        [Header("MachineSettings")] 
        [SerializeField] private int prepTime;
        [SerializeField] private Recipe product;

        private MicrowaveableFood _currentMicrowaveFood; //Microwaved food
        private Coroutine _currentCoroutine;
        
        public enum MicrowaveState
        {
            Idle,
            Working,
            Waiting,
            Finished
        }
        public MicrowaveState currentState;


        private void UpdateMicrowaveState(MicrowaveState state)
        {
            currentState = state;
            if (state == MicrowaveState.Idle)
            {
                _currentMicrowaveFood = null;
                
            }
            if (state == MicrowaveState.Working)
            {
                _currentCoroutine = StartCoroutine(Microwaving());
            }
            if (state == MicrowaveState.Waiting)
            {
                
            }
            if (state == MicrowaveState.Finished)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
                _currentMicrowaveFood!.gameObject.SetActive(false);
                Instantiate(product, microwaveTransform.position, Quaternion.identity);
            }
        }

        public override void Interact()
        {
            if (currentState == MicrowaveState.Idle && _currentMicrowaveFood )
            {
                UpdateMicrowaveState(MicrowaveState.Working);
            }
            if (currentState == MicrowaveState.Waiting)
            {
                UpdateMicrowaveState(MicrowaveState.Finished);
            }
        }
        
        private void MoveToPos(MicrowaveableFood obj, Transform targetPos)
        {
            obj.transform.DOMove(targetPos.position, 0.1f);
            obj.transform.DORotate(targetPos.eulerAngles, 0.1f);
        }

        private IEnumerator Microwaving()
        {
            while (currentState == MicrowaveState.Working)
            {
                yield return new WaitForSeconds(prepTime);
                UpdateMicrowaveState(MicrowaveState.Waiting);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MicrowaveableFood food) && !_currentMicrowaveFood && !food.Food.IsPickedUp)
            {
                food.GetComponent<Collider>().gameObject.layer = 0;
                MoveToPos(food,microwaveTransform);
                _currentMicrowaveFood = food;
                food.Food.RigidBody.isKinematic = true;
            }
        }
    }
}

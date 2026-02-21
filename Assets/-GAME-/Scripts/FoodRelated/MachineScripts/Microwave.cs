using System;
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
        [SerializeField] private Transform microwaveHandle;
        [Header("MachineSettings")] 
        [SerializeField] private int prepTime;

        [NonSerialized] public MicrowaveableFood CurrentMicrowaveFood; //Microwaved food
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
               
            }
            if (state == MicrowaveState.Working)
            {
                _currentCoroutine = StartCoroutine(Microwaving());
            }
            if (state == MicrowaveState.Waiting)
            {
                //Play sound
            }
            if (state == MicrowaveState.Finished)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
        }

        public override void Interact()
        {
            if (currentState == MicrowaveState.Idle && CurrentMicrowaveFood )
            {
                microwaveHandle.transform.DORotate(new Vector3(0, 0, 0), 0.5f)
                    .OnComplete((() => {UpdateMicrowaveState(MicrowaveState.Working);}));
                
            }
            if (currentState == MicrowaveState.Waiting)
            {
                microwaveHandle.transform.DORotate(new Vector3(0, 120, 0), 0.5f)
                    .OnComplete((() => {UpdateMicrowaveState(MicrowaveState.Finished);}));
            }
        }

        public void ResetState()
        {
            CurrentMicrowaveFood!.gameObject.SetActive(false);
            CurrentMicrowaveFood = null;
            currentState = MicrowaveState.Idle;
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
            if (other.TryGetComponent(out MicrowaveableFood food) && !CurrentMicrowaveFood && !food.Food.IsPickedUp)
            {
                food.GetComponent<Collider>().gameObject.layer = 0;
                MoveToPos(food,microwaveTransform);
                CurrentMicrowaveFood = food;
                food.Food.RigidBody.isKinematic = true;
            }
        }
    }
}

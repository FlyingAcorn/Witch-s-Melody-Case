using System;
using System.Collections;
using System.Collections.Generic;
using _GAME_.Scripts.FoodRelated.FriedFood;
using DG.Tweening;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated.MachineScripts
{
    public class Fryer : Interactable
    {
        [Header("References")] [SerializeField]
        private Transform fryerTransform;
        [Header("MachineSettings")] 
        [SerializeField] private int fryTimer;
        [SerializeField] private int burnTimer;



        [NonSerialized]public FryableFood CurrentFriableFood;
        private Coroutine _currentCoroutine;
        
        public enum FryerStates
        {
            Idle,
            Working,
            Fried,
            Burned,
            ResultBurned,
            ResultFried
        }
        public FryerStates currentState;


        private void UpdateFryerState(FryerStates state)
        {
            currentState = state;
            if (state == FryerStates.Idle)
            {
                CurrentFriableFood = null;
            }
            if (state == FryerStates.Working)
            {
                //Play anim and whatnot
                _currentCoroutine = StartCoroutine(Frying());

            }
            if (state == FryerStates.Fried)
            {
                CurrentFriableFood!.ChangeMaterial(1);
            }
            if (state == FryerStates.Burned)
            {
                CurrentFriableFood!.ChangeMaterial(2);
            }
            if (state == FryerStates.ResultBurned) // burned ise anim, isim değiş 
            {
                
            }
            if (state == FryerStates.ResultFried) //anim wait for pickup
            {
                
            }
        }

        public override void Interact()
        {
            if (currentState == FryerStates.Idle && CurrentFriableFood )
            {
                UpdateFryerState(FryerStates.Working);
            }
            else if (currentState == FryerStates.Fried)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
                UpdateFryerState(FryerStates.ResultFried);
            }
            else if (currentState == FryerStates.Burned)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
                UpdateFryerState(FryerStates.ResultBurned);
            }
            else if (currentState == FryerStates.ResultBurned)
            {
                // clean residue
                UpdateFryerState(FryerStates.Idle);
            }
        }

        public void ResetState()
        {
          UpdateFryerState(FryerStates.Idle);   
        }
        
        private void MoveToPos(FryableFood obj, Transform targetPos)
        {
            obj.transform.DOMove(targetPos.position, 0.1f);
            obj.transform.DORotate(targetPos.eulerAngles, 0.1f);
        }

        private IEnumerator Frying()
        {
            while (currentState == FryerStates.Working)
            {
                yield return new WaitForSeconds(fryTimer);
                UpdateFryerState(FryerStates.Fried);
            }

            while (currentState == FryerStates.Fried)
            {
                yield return new WaitForSeconds(burnTimer);
                UpdateFryerState(FryerStates.Burned);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out FryableFood food) && !CurrentFriableFood && !food.Food.IsPickedUp)
            {
                MoveToPos(food,fryerTransform );
                CurrentFriableFood = food;
                food.Food.RigidBody.isKinematic = true;
                CurrentFriableFood.gameObject.SetActive(false);
            }
        }
    }
}

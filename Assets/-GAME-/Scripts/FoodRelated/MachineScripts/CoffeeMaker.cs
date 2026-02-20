using System.Collections;
using _GAME_.Scripts.FoodRelated.Coffee;
using _GAME_.Scripts.FoodRelated.RecipeObject;
using DG.Tweening;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated.MachineScripts
{
    public class CoffeeMaker : Interactable
    {
        [SerializeField] private Transform cupHoldPos;
        [SerializeField] private Transform lidHoldPos;
        [SerializeField] private int fillTimer;
        [SerializeField] private CoffeMakerPodArea coffeePod;
        [SerializeField] private Recipe finishedProduct;
        
        private CoffeeCup _cupInserted;
        private Coroutine _currentCoroutine;

        private enum CoffeeMakerStates
        {
            Idle,
            Working,
            Waiting,
            Finished
        }
        [SerializeField] private CoffeeMakerStates currentState;


        private void UpdateCoffeeMakerState(CoffeeMakerStates state)
        {
            currentState = state;

            if (state == CoffeeMakerStates.Idle)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }

            if (state == CoffeeMakerStates.Working)
            {
                _currentCoroutine = StartCoroutine(MakeCoffee());
            }

            if (state == CoffeeMakerStates.Waiting)
            {
                coffeePod.PodInserted.gameObject.SetActive(false);
                coffeePod.PodInserted = null;
            }
            if (state == CoffeeMakerStates.Finished)
            {
                _cupInserted!.gameObject.SetActive(false);
                _cupInserted = null;
                Instantiate(finishedProduct, cupHoldPos.position, Quaternion.identity);
            }
            
        }
        
        private void MoveToPos(Interactable obj, Transform targetPos)
        {
            obj.transform.DOMove(targetPos.position, 0.1f).OnComplete(() =>
            {
                if (currentState == CoffeeMakerStates.Waiting) UpdateCoffeeMakerState(CoffeeMakerStates.Finished);
            });
            obj.transform.DORotate(targetPos.eulerAngles, 0.1f);
        }

        public override void Interact()
        {
            if (currentState == CoffeeMakerStates.Idle && _cupInserted && coffeePod.PodInserted)
            {
                UpdateCoffeeMakerState(CoffeeMakerStates.Working);
            }
        }

        private IEnumerator MakeCoffee()
        {
           yield return new WaitForSeconds(fillTimer); // animation and whatnot will play
           UpdateCoffeeMakerState(CoffeeMakerStates.Waiting);
           
        }
        
        private void OnTriggerStay(Collider obj)  // cup ve lid alınamıyacak içeri koyuldugunda
        {
            if (obj.TryGetComponent(out CoffeeCup cup) && !_cupInserted && !cup.IsPickedUp)
            {
                _cupInserted = cup;
                cup.GetComponent<Collider>().gameObject.layer = 0;
                cup.RigidBody.isKinematic = true;
                MoveToPos(cup,cupHoldPos);
            }
            else if (obj.TryGetComponent(out CoffeeCupLid lid) && currentState == CoffeeMakerStates.Waiting && !lid.IsPickedUp)
            {
                lid.transform.parent = cupHoldPos.transform; // you might change it
                lid.RigidBody.isKinematic = true;
                MoveToPos(lid,lidHoldPos);
            }
        }
    }
}

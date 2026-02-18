using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated
{
    public class CookableObj : MonoBehaviour,IInteractable
    {
        private enum FoodState
        {
            Raw,
            Cooked,
            Burned
        }
        [SerializeField] private Renderer myRenderer;
        [Header("ObjectConfigurations")] 
        [SerializeField] private string objectInteractMessage;
        [SerializeField] private bool isInteractable;
        [SerializeField] private bool canBePickedUp;
        [SerializeField] private bool isSharp;
        [Header("FoodConfigurations")] 
        [SerializeField] private int cookTime;
        [SerializeField] private int burnTime;
        [SerializeField] private FoodState currentFoodState;
        [SerializeField] private List<Material> foodStateMaterials;
        private Coroutine _currentCoroutine;
        private Rigidbody _rigidbody;
        private Collider _interactCollider;
        
        public string InteractMessage => objectInteractMessage;
        public GameObject InteractObject => gameObject;
        public Rigidbody InteractRigidbody => _rigidbody;
        public Collider InteractCollider => _interactCollider;
        public bool IsInteractable => isInteractable;
        public bool CanBePickedUp => canBePickedUp;
        public bool IsSharp => isSharp;
        public bool IsPickedUp { get; set; }

        public void Interact()
        {
        }

        private void Awake()
        {
            UpdateFoodState(FoodState.Raw);
            _rigidbody = GetComponent<Rigidbody>();
            _interactCollider  = GetComponent<Collider>();
        }

        private void Start()
        {
        }

        private void UpdateFoodState(FoodState state)
        {
            currentFoodState = state;
            if (currentFoodState == FoodState.Raw)
            {
                objectInteractMessage = nameof(FoodState.Raw)+gameObject.name;
                
            }
            if (currentFoodState == FoodState.Cooked)
            {
                objectInteractMessage = nameof(FoodState.Cooked)+gameObject.name;
            }

            if (currentFoodState == FoodState.Burned)
            {
                objectInteractMessage = nameof(FoodState.Burned)+gameObject.name;
            }
            myRenderer.material = foodStateMaterials[(int)state];
        }

        private IEnumerator Cooking()
        {
            while (currentFoodState == FoodState.Raw)
            {
                yield return new WaitForSeconds(cookTime);
                if (currentFoodState == FoodState.Raw) UpdateFoodState(FoodState.Cooked);
                
            }

            while (currentFoodState == FoodState.Cooked)
            {
                yield return new WaitForSeconds(burnTime); 
                if (currentFoodState == FoodState.Cooked) UpdateFoodState(FoodState.Burned);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Grill _))
            {
                _currentCoroutine = StartCoroutine(Cooking());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Grill _)) return;
            if (_currentCoroutine == null) return;
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }
}

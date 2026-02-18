using UnityEngine;

namespace _GAME_.Scripts.FoodRelated
{
    public class RecipeFood : MonoBehaviour,IInteractable
    {
        [Header("ObjectConfigurations")] 
        [SerializeField] private string objectInteractMessage;
        [SerializeField] private bool isInteractable;
        [SerializeField] private bool canBePickedUp;
        [SerializeField] private bool isSharp;
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

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _interactCollider  = GetComponent<Collider>();
        }
        public void Interact()
        {
        }
    }
}

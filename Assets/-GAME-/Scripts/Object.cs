using System;
using UnityEngine;

namespace _GAME_.Scripts
{
    public class Object : MonoBehaviour,IInteractable
    {
        public string InteractMessage => objectInteractMessage;
        public GameObject InteractObject => gameObject;
        public Collider InteractCollider => _collider;
        public bool IsInteractable => isInteractable;
        public bool CanBePickedUp => canBePickedUp;
        public bool IsSharp => isSharp;
        public Rigidbody InteractRigidbody => _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void Interact()
        {
            Debug.Log("yaho");
        }
        
        [SerializeField] private string objectInteractMessage;
        [SerializeField] private bool isInteractable;
        [SerializeField] private bool canBePickedUp;
        [SerializeField] private bool isSharp;
        private Rigidbody _rigidbody;
        private Collider _collider;
    }
}

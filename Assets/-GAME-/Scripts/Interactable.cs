using System;
using UnityEngine;

namespace _GAME_.Scripts
{
    public abstract class Interactable : MonoBehaviour
    {
        [Header("ObjectConfigurations")] 
        [SerializeField] public string objectInteractMessage;
        [SerializeField] public bool isInteractable;
        [SerializeField] public bool canBePickedUp;
        [NonSerialized] public bool IsPickedUp;
        [NonSerialized] public bool CanBeThrown = true;
        [SerializeField] public bool isSharp;
        [NonSerialized] public Rigidbody RigidBody;
        
        protected virtual void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }
        
        public virtual void Interact()
        {
            
        }
    }
}

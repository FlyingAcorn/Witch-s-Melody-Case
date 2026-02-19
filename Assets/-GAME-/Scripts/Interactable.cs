using System;
using UnityEngine;

namespace _GAME_.Scripts
{
    public abstract class Interactable : MonoBehaviour
    {
        [Header("ObjectConfigurations")] 
        public string objectInteractMessage;
        public bool isInteractable;
        public bool canBePickedUp;
        [NonSerialized] public bool IsPickedUp;
        [NonSerialized] public bool CanBeThrown = true;
        public bool isSharp;
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

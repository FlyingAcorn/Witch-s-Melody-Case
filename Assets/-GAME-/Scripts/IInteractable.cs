using UnityEngine;

namespace _GAME_.Scripts
{
    public interface IInteractable
    {
        public string InteractMessage { get; }
        public GameObject InteractObject { get; }
        public Rigidbody InteractRigidbody { get; }
        public bool IsInteractable { get; }
        public bool CanBePickedUp { get; }
        public bool IsSharp { get; }
        public bool IsPickedUp { get; set; }
        public void Interact();
    }
}

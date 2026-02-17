using UnityEngine;

namespace _GAME_.Scripts
{
    public interface IInteractable
    {
        public string InteractMessage { get; }
        public GameObject InteractObject { get; }
    }
}

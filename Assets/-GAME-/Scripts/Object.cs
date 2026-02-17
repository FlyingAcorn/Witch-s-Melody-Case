using UnityEngine;

namespace _GAME_.Scripts
{
    public class Object : MonoBehaviour,IInteractable
    {
        public string InteractMessage => objectInteractMessage;
        public GameObject InteractObject => gameObject;
        [SerializeField] private string objectInteractMessage;
    }
}

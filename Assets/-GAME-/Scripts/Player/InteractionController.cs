using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _GAME_.Scripts.Player
{
    public class InteractionController : MonoBehaviour
    {
        [Header("InteractConfigurations")] 
        [SerializeField] private Camera playerCamera;
        [SerializeField] private TextMeshProUGUI interactionText;
        [SerializeField] private float interactionRange = 5f;
        [SerializeField] private float throwForce = 500f;
        [SerializeField] private Transform objHoldPos;
        private Interactable _currentTargetedInteractable;
        private Interactable _pickedInteractable;
        [SerializeField] private InputActionAsset myInputActionAsset;
        private InputAction _pickupAction;
        private InputAction _throwAction;
        private InputAction _interactAction;
        [SerializeField] private LayerMask objectLayer;
        
        //Events
        public UnityEvent<int> interactionState;// 0 normal, 1 picked the interactable

        private void Awake()
        {
            _pickupAction = myInputActionAsset.FindAction("PickUp");
            _throwAction = myInputActionAsset.FindAction("Throw");
            _interactAction = myInputActionAsset.FindAction("Interact");
        }

        private void Update()
        {
            UpdateCurrentInteractable();
            UpdateInteractionText();
            CheckForInteractionInput();
        }

        private void FixedUpdate()
        {
            if (_pickedInteractable != null)
            {
                MoveObject();
            }
        }

        private void CheckForInteractionInput()
        {
            if (_pickupAction.WasReleasedThisFrame() && _currentTargetedInteractable != null && _pickedInteractable == null )
            {
                if (_currentTargetedInteractable.canBePickedUp) 
                {
                    _pickedInteractable= _currentTargetedInteractable;
                    PickUpObject(_pickedInteractable.gameObject);
                    
                }
                else if (_currentTargetedInteractable.isInteractable)
                {
                    _currentTargetedInteractable.Interact();
                }
            }
            else if (_pickupAction.WasReleasedThisFrame() && _pickedInteractable != null)
            {
                DropObject();
                
            }
            if (_pickedInteractable == null) return;
            if (_throwAction.WasReleasedThisFrame())
            {
                ThrowObject();
              
            }
            if (_interactAction.IsPressed()&& _pickedInteractable.isInteractable)
            {
                _pickedInteractable.Interact();
            }
        }

        private void UpdateInteractionText()
        {
            if (_currentTargetedInteractable == null || _currentTargetedInteractable == _pickedInteractable )
            {
                interactionText.text = String.Empty;
                return;
            }
            interactionText.text = _currentTargetedInteractable.objectInteractMessage;
        }

        private void UpdateCurrentInteractable()
        {
            var ray = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            Physics.Raycast(ray, out var hit, interactionRange, objectLayer);
            _currentTargetedInteractable = hit.collider?.GetComponent<Interactable>();
        }
        
        void PickUpObject(GameObject pickUpObj)
        {
            if (pickUpObj.TryGetComponent(out Rigidbody rb)) 
            {
                _pickedInteractable.IsPickedUp = true;
                rb.useGravity =false;
                rb.transform.parent = objHoldPos.transform;
                Physics.IgnoreCollision(pickUpObj.GetComponent<Collider>(), GetComponent<CharacterController>(), true);
                interactionState?.Invoke(1); 
            }
        }
        void DropObject()
        {
            _pickedInteractable.RigidBody.useGravity = true;
            _pickedInteractable.RigidBody.linearVelocity = Vector3.zero;
            _pickedInteractable.IsPickedUp = false;
            Physics.IgnoreCollision(_pickedInteractable.gameObject.GetComponent<Collider>(), GetComponent<CharacterController>(), false);
            _pickedInteractable.gameObject.transform.parent = null; 
            _pickedInteractable = null; 
            interactionState?.Invoke(0);
        }
        void MoveObject()
        {
            _pickedInteractable.gameObject.transform.position =
                Vector3.MoveTowards(_pickedInteractable.gameObject.transform.position,objHoldPos.position,10f * Time.fixedDeltaTime);
        }
        void ThrowObject()
        {
            if (!_pickedInteractable.CanBeThrown) return;
            _pickedInteractable.RigidBody.linearVelocity = Vector3.zero;
            _pickedInteractable.IsPickedUp = false;
            _pickedInteractable.RigidBody.useGravity = true;
            Physics.IgnoreCollision(_pickedInteractable.gameObject.GetComponent<Collider>(), GetComponent<CharacterController>(), false);
            _pickedInteractable.gameObject.transform.parent = null;
            _pickedInteractable.RigidBody.AddForce(transform.forward * throwForce); // sharpsa farklı yapsın
            _pickedInteractable = null;
            interactionState?.Invoke(0);
        }
    }
}

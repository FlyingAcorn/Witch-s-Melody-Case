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
        [SerializeField] private int targetLayer;
        [SerializeField] private Transform objHoldPos;
        private IInteractable _currentTargetedInteractable;
        private IInteractable _pickedInteractable;
        [SerializeField] private InputActionAsset myInputActionAsset;
        private InputAction _pickupAction;
        private InputAction _throwAction;
        private InputAction _interactAction;
        private int _previousLayer;
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
                if (_currentTargetedInteractable.CanBePickedUp) 
                {
                    _pickedInteractable= _currentTargetedInteractable;
                    PickUpObject(_pickedInteractable.InteractObject);
                    interactionState?.Invoke(1); 
                }
                else if (_currentTargetedInteractable.IsInteractable)
                {
                    _currentTargetedInteractable.Interact();
                }
            }
            else if (_pickupAction.WasReleasedThisFrame() && _pickedInteractable != null)
            {
                DropObject();
                interactionState?.Invoke(0);
            }
            if (_pickedInteractable == null) return;
            if (_throwAction.WasReleasedThisFrame())
            {
                ThrowObject();
                interactionState?.Invoke(0);
            }
            if (_interactAction.IsPressed()&& _pickedInteractable.IsInteractable)
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
            interactionText.text = _currentTargetedInteractable.InteractMessage;
        }

        private void UpdateCurrentInteractable()
        {
            var ray = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            Physics.Raycast(ray, out var hit, interactionRange, objectLayer);
            _currentTargetedInteractable = hit.collider?.GetComponent<IInteractable>();
        }
        
        void PickUpObject(GameObject pickUpObj)
        {
            if (pickUpObj.TryGetComponent(out Rigidbody rb)) 
            {
                _pickedInteractable.IsPickedUp = true;
                rb.useGravity =false;
                rb.transform.parent = objHoldPos.transform;
                _previousLayer = pickUpObj.layer;
                pickUpObj.layer = targetLayer;
                Physics.IgnoreCollision(pickUpObj.GetComponent<Collider>(), GetComponent<CharacterController>(), true);
            }
        }
        void DropObject()
        {
            _pickedInteractable.InteractRigidbody.useGravity = true;
            _pickedInteractable.InteractRigidbody.linearVelocity = Vector3.zero;
            _pickedInteractable.IsPickedUp = false;
            Physics.IgnoreCollision(_pickedInteractable.InteractObject.GetComponent<Collider>(), GetComponent<CharacterController>(), false);
            _pickedInteractable.InteractObject.layer = _previousLayer; 
            _pickedInteractable.InteractObject.transform.parent = null; 
            _pickedInteractable = null; 
        }
        void MoveObject()
        {
            _pickedInteractable.InteractObject.transform.position =
                Vector3.MoveTowards(_pickedInteractable.InteractObject.transform.position,objHoldPos.position,10f * Time.fixedDeltaTime);
        }
        void ThrowObject()
        {
            _pickedInteractable.InteractRigidbody.linearVelocity = Vector3.zero;
            _pickedInteractable.IsPickedUp = false;
            _pickedInteractable.InteractRigidbody.useGravity = true;
            Physics.IgnoreCollision(_pickedInteractable.InteractObject.GetComponent<Collider>(), GetComponent<CharacterController>(), false);
            _pickedInteractable.InteractObject.layer = _previousLayer;
            _pickedInteractable.InteractObject.transform.parent = null;
            _pickedInteractable.InteractRigidbody.AddForce(transform.forward * throwForce); // sharpsa farklı yapsın
            _pickedInteractable = null;
        }
    }
}

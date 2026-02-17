using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _GAME_.Scripts
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

        private void CheckForInteractionInput()
        {
            if (_pickupAction.WasReleasedThisFrame() && _currentTargetedInteractable != null)
            {
                if (_pickedInteractable == null)
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
                else 
                {
                    StopClipping();
                    DropObject();
                    interactionState?.Invoke(0);
                }
            }
            if (_pickedInteractable != null)
            {
                MoveObject();
                if (_throwAction.WasReleasedThisFrame())
                {
                 StopClipping();
                 ThrowObject();
                 interactionState?.Invoke(0);
                }
                if (_interactAction.IsPressed()&& _pickedInteractable.IsInteractable)
                {
                    _pickedInteractable.Interact();
                }
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
            Physics.Raycast(ray, out var hit, interactionRange);
            _currentTargetedInteractable = hit.collider?.GetComponent<IInteractable>();
        }
        
        void PickUpObject(GameObject pickUpObj)
        {
            if (pickUpObj.TryGetComponent(out Rigidbody rb)) 
            {
                rb.isKinematic = true;
                rb.transform.parent = objHoldPos.transform;
                _previousLayer = pickUpObj.layer;
                pickUpObj.layer = targetLayer;
                Physics.IgnoreCollision(pickUpObj.GetComponent<Collider>(), GetComponent<CharacterController>(), true);
            }
        }
        void DropObject()
        {
            _pickedInteractable.InteractObject.TryGetComponent(out Rigidbody rb);
            Physics.IgnoreCollision(_pickedInteractable.InteractObject.GetComponent<Collider>(), GetComponent<CharacterController>(), false);
            _pickedInteractable.InteractObject.layer = _previousLayer; 
            rb.isKinematic = false;
            _pickedInteractable.InteractObject.transform.parent = null; 
            _pickedInteractable = null; 
        }
        void StopClipping()
        {
            var clipRange = Vector3.Distance(_pickedInteractable.InteractObject.transform.position, transform.position);
            RaycastHit[] hits;
            // ReSharper disable once Unity.PreferNonAllocApi
            hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
            if (hits.Length > 1)
            {
                _pickedInteractable.InteractObject.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
            }
        }
        void MoveObject()
        {
            _pickedInteractable.InteractObject.transform.position = objHoldPos.transform.position;
        }
        void ThrowObject()
        {
            _pickedInteractable.InteractObject.TryGetComponent(out Rigidbody rb);
            Physics.IgnoreCollision(_pickedInteractable.InteractObject.GetComponent<Collider>(), GetComponent<CharacterController>(), false);
            _pickedInteractable.InteractObject.layer = _previousLayer;
            rb.isKinematic = false;
            _pickedInteractable.InteractObject.transform.parent = null;
            rb.AddForce(transform.forward * throwForce); // sharpsa farklı yapsın
            _pickedInteractable = null;
        }
    }
}

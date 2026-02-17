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
        private IInteractable _selectedInteractable;
        [SerializeField] private InputActionAsset myInputActionAsset;
        private InputAction _pickupAction;
        private InputAction _throwAction;
        private int _previousLayer;
        
        //Events
        public UnityEvent<int> interactionState;// 0 normal, 1 picked the interactable

        private void Awake()
        {
            _pickupAction = myInputActionAsset.FindAction("PickUp");
            _throwAction = myInputActionAsset.FindAction("Throw");
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
                if (_selectedInteractable == null)
                {
                    _selectedInteractable= _currentTargetedInteractable;
                    PickUpObject(_selectedInteractable.InteractObject);
                    interactionState?.Invoke(1);
                }
                else
                {
                    StopClipping();
                    DropObject();
                    interactionState?.Invoke(0);
                }
            }
            if (_selectedInteractable != null)
            {
                MoveObject();
                if (_throwAction.WasReleasedThisFrame())
                {
                 StopClipping();
                 ThrowObject();
                 interactionState?.Invoke(0);
                }
            }
        }

        private void UpdateInteractionText()
        {
            if (_currentTargetedInteractable == null || _currentTargetedInteractable == _selectedInteractable )
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
            _selectedInteractable.InteractObject.TryGetComponent(out Rigidbody rb);
            Physics.IgnoreCollision(_selectedInteractable.InteractObject.GetComponent<Collider>(), GetComponent<CharacterController>(), false);
            _selectedInteractable.InteractObject.layer = _previousLayer; 
            rb.isKinematic = false;
            _selectedInteractable.InteractObject.transform.parent = null; 
            _selectedInteractable = null; 
        }
        void StopClipping()
        {
            var clipRange = Vector3.Distance(_selectedInteractable.InteractObject.transform.position, transform.position);
            RaycastHit[] hits;
            // ReSharper disable once Unity.PreferNonAllocApi
            hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
            if (hits.Length > 1)
            {
                _selectedInteractable.InteractObject.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
            }
        }
        void MoveObject()
        {
            _selectedInteractable.InteractObject.transform.position = objHoldPos.transform.position;
        }
        void ThrowObject()
        {
            _selectedInteractable.InteractObject.TryGetComponent(out Rigidbody rb);
            Physics.IgnoreCollision(_selectedInteractable.InteractObject.GetComponent<Collider>(), GetComponent<CharacterController>(), false);
            _selectedInteractable.InteractObject.layer = _previousLayer;
            rb.isKinematic = false;
            _selectedInteractable.InteractObject.transform.parent = null;
            rb.AddForce(transform.forward * throwForce);
            _selectedInteractable = null;
        }
    }
}

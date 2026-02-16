using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _GAME_.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera myCamera;
        [SerializeField] private Transform myHead;
        [SerializeField] private InputActionAsset myInputActionAsset;
        
        [Header("Configurations")] 
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 10f;
        [SerializeField] private float mouseSensitivity = 0.1f;
        [SerializeField] private float upDownLookRange = 80f;
        [SerializeField] private float forceMagnitude;
        [SerializeField] private float gravityMagnitude;
        
        

        // inputs
        private InputAction _moveAction;
        private InputAction _sprintAction;
        private InputAction _lookAction;
        
        private Vector2 _moveAmount;
        private Vector2 _lookAmount;
        private float _verticalRotation;
        
        private void OnEnable()
        {
            myInputActionAsset.FindActionMap("Gameplay").Enable();
        }

        private void OnDisable()
        {
            myInputActionAsset.FindActionMap("Gameplay").Disable();
        }
        
        private void Awake()
        {
            _moveAction = myInputActionAsset.FindAction("Move");
            _sprintAction = myInputActionAsset.FindAction("Sprint");
            _lookAction = myInputActionAsset.FindAction("Look");
        }
        
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            _moveAmount = _moveAction.ReadValue<Vector2>();
            _lookAmount = _lookAction.ReadValue<Vector2>();
            Looking();
            
        }

        private void FixedUpdate()
        {
            Moving();
        }
        private void Moving()
        {
            var worldDirection = CalculateWorldDirection();
            var speed = _sprintAction.IsPressed() ? runSpeed : walkSpeed;
            worldDirection.y += Physics.gravity.y * gravityMagnitude * Time.deltaTime; // change this when you add jumping
            characterController.Move(worldDirection * Time.deltaTime * speed);
        }
        private Vector3 CalculateWorldDirection()
        {
            Vector3 inputDirection = new Vector3(_moveAmount.x, 0f, _moveAmount.y);
            Vector3 worldDirection = transform.TransformDirection(inputDirection);
            return worldDirection.normalized;
        }

        private void Looking()
        {
            float mouseXRotation = _lookAmount.x * mouseSensitivity;
            float mouseYRotation = _lookAmount.y * mouseSensitivity;
            
            ApplyHorizontalRotation(mouseXRotation);
            ApplyVerticalRotation(mouseYRotation);
        }
        
        private void ApplyHorizontalRotation(float rotationAmount)
        {
            transform.Rotate(0, rotationAmount, 0);
        }
        
        private void ApplyVerticalRotation(float rotationAmount)
        {
           _verticalRotation = Mathf.Clamp(_verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
            myCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body != null)
            {
                Vector3 forceDirection = body.gameObject.transform.position - transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();
                body.AddForceAtPosition(forceDirection * forceMagnitude,transform.position,ForceMode.Impulse);
            }
        }
    }
}

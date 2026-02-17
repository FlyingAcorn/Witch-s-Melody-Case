using UnityEngine;
using UnityEngine.InputSystem;

namespace _GAME_.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera myCamera;
        [SerializeField] private InputActionAsset myInputActionAsset;
        [SerializeField] private Transform cameraHolder;
        
        

        [Header("MovementConfigurations")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 10f;
        [SerializeField] private float mouseSensitivity = 0.1f;
        [SerializeField] private float upDownLookRange = 80f;
        private Vector2 _moveAmount;
        private Vector2 _lookAmount;
        private float _verticalRotation;

        [Header("HeadBobConfigurations")] 
        [SerializeField, Range(0.001f, 0.02f)] private float amplitude = 0.005f;
        [SerializeField, Range(0, 30)] private float frequency = 10f;
        private Vector3 _startPos;
        
        [Header("CameraTiltConfigurations")] 
        [SerializeField] private float tiltAmount;
        [SerializeField] private float tiltStartSpeed;
        [SerializeField] private float tiltEndSpeed;
        private float _currentTilt;
        private float _targetTilt;
        
        [Header("OtherConfigurations")] //

        // inputs
        private InputAction _moveAction;
        private InputAction _sprintAction;
        private InputAction _lookAction;
        

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
            _startPos = myCamera.transform.localPosition;
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
            characterController.Move(worldDirection * (Time.deltaTime * speed));
            if (characterController.velocity.magnitude != 0) PlayMotion(FootStepMotion());
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
            CameraTilt();
            ResetPosition();
        }

        private void ApplyHorizontalRotation(float rotationAmount)
        {
            transform.Rotate(0, rotationAmount, 0);
        }

        private void ApplyVerticalRotation(float rotationAmount)
        {
            _verticalRotation = Mathf.Clamp(_verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
            myCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, _currentTilt);
        }

        private Vector3 FootStepMotion()
        {
            Vector3 pos = Vector3.zero;
            pos.y += Mathf.Sin(Time.time * frequency) * amplitude * (_sprintAction.IsPressed() ? 2f : 1f);
            pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2 * (_sprintAction.IsPressed() ? 2f : 1f); // bunu göster sor
            return pos;
        }

        private void PlayMotion(Vector3 motion)
        {
            myCamera.transform.localPosition += motion;
        }

        private void ResetPosition()
        {
            if (myCamera.transform.localPosition == _startPos) return;
            myCamera.transform.localPosition =
                Vector3.Lerp(myCamera.transform.localPosition, _startPos, 5f * Time.deltaTime);
        }

        private void CameraTilt()
        {
            bool leftStrafe = _moveAmount.x < 0f;
            bool rightStrafe = _moveAmount.x > 0f;
            if (leftStrafe && !rightStrafe)
            {
                _targetTilt = tiltAmount;
            }
            else if (rightStrafe && !leftStrafe)
            {
                _targetTilt = -tiltAmount;
            }
            else
            {
                _targetTilt = 0f;
            }

            float smoothTilt;
            if (_targetTilt == 0f)
            {
                smoothTilt = tiltEndSpeed;
            }
            else
            {
                smoothTilt = tiltStartSpeed;
            }
            _currentTilt = Mathf.Lerp(_currentTilt, _targetTilt, smoothTilt *Time.deltaTime);
        }
    }
}
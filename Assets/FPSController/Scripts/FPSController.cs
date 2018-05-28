using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

namespace B2BGFPS
{

    public enum PlayerMoveStatus { NotMoving, Walking, Running, NotGrounded, Landing }

    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {

        // Inspector Assigned
        [SerializeField] private float _walkSpeed = 1f;
        [SerializeField] private float _runSpeed = 5f;
        [SerializeField] private float _jumpSpeed = 8f;
        [SerializeField] private float _stickToGroundForce = 5f;
        [SerializeField] private float _gravityMultiplier = 2.5f;

        // Use of MouseLook Class for mouse Input -> Camera Look Control
        [SerializeField] private MouseLook _mouseLook;

        private Camera _camera = null;
        private bool _jumpButtonPressed = false; // Safe input register (Update Method) to interact with Physics Objects movend in FixedUpdate
        private Vector2 _inputVector = Vector2.zero;
        private Vector3 _moveDirection = Vector2.zero;
        private bool _previouslyGrounded = false;
        private bool _isWalking = true;
        private bool _isJumping = false;

        // Timers
        private float _fallingTimer = 0f;
        private CharacterController _characterController = null;
        private PlayerMoveStatus _movementStatus = PlayerMoveStatus.NotMoving;

        // Public properties
        public PlayerMoveStatus movementStatus { get { return _movementStatus; } }
        public float walkSpeed { get { return _walkSpeed; } }
        public float runSpeed { get { return _runSpeed; } }

        // Use this for initialization
        void Start()
        {
            // Initial state of the pointer
            Cursor.lockState = CursorLockMode.Locked;

            _characterController = GetComponent<CharacterController>();
            _camera = Camera.main;

            // Initialization of some variables
            _movementStatus = PlayerMoveStatus.NotMoving;
            _fallingTimer = 0f;

            // Mouse Look Initialization
            _mouseLook.Init(transform, _camera.transform);
            
        }

        // Update is called once per frame
        void Update()
        {
            // in case of character falling, increase the fallingTimer
            if (_characterController.isGrounded) _fallingTimer = 0f;
            else _fallingTimer += Time.deltaTime;

            // Allow Mouse Look a chance to process mouse and rotate camera
            if (Time.timeScale > Mathf.Epsilon) _mouseLook.LookRotation(transform, _camera.transform);

            // Validate Jump button, and we read it from here to not miss it in other update function
            if (!_jumpButtonPressed) _jumpButtonPressed = Input.GetButtonDown("Jump");

            if (!_previouslyGrounded && _characterController.isGrounded)
            {
                if (_fallingTimer > 0.5f)
                {
                    // TODO: Play Landing sound or calculate damage
                }
                _moveDirection.y = 0f;
                _isJumping = false;
                _movementStatus = PlayerMoveStatus.Landing;
            }
            else if (!_characterController.isGrounded) _movementStatus = PlayerMoveStatus.NotGrounded;
            else if (_characterController.velocity.sqrMagnitude < 0.01f) _movementStatus = PlayerMoveStatus.NotMoving;
            else if (_isWalking) _movementStatus = PlayerMoveStatus.Walking;
            else _movementStatus = PlayerMoveStatus.Running;

            _previouslyGrounded = _characterController.isGrounded;

            // Cursor release
            if (Input.GetKey(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
        }


        protected void FixedUpdate()
        {
            // Read Input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");
            bool wasWalking = _isWalking;
            _isWalking = !Input.GetKey(KeyCode.LeftShift);

            // Set desired speed based on walking flag
            float speed = _isWalking ? _walkSpeed : _runSpeed;
            _inputVector = new Vector2(horizontal, vertical);

            // Normalize input vector if exceeds 1
            if (_inputVector.sqrMagnitude > 1) _inputVector.Normalize();

            // Get the move vector along the x and y axis
            Vector3 desiredMove = transform.forward * _inputVector.y + transform.right * _inputVector.x;

            // Get the normal of the touched surface to move along it
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, _characterController.radius, Vector3.down, out hitInfo, _characterController.height / 2f, 1))
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            // Set the desired speed of walking or running
            _moveDirection.x = desiredMove.x * speed;
            _moveDirection.z = desiredMove.z * speed;

            // Check if player is grounded
            if (_characterController.isGrounded)
            {
                // To keep control we apply force to down
                _moveDirection.y = -_stickToGroundForce;

                // Also we validate if the jump button was pressed
                if (_jumpButtonPressed)
                {
                    _moveDirection.y = _jumpSpeed;
                    _jumpButtonPressed = false;
                    _isJumping = true;
                }

            }
            else
            {
                // if the character is on the air, appliy the gravity multiplier
                _moveDirection += Physics.gravity * _gravityMultiplier * Time.fixedDeltaTime;
            }

            // Move the character controller
            _characterController.Move(_moveDirection * Time.fixedDeltaTime);
        }
    }
}



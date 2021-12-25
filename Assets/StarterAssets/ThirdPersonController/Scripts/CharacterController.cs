using System;
using UnityEngine;
using UnityEngine.InputSystem;

 /* Note: animations are called via the controller for both the character and capsule using animator null checks
  */

 namespace StarterAssets
{
    //[RequireComponent(typeof(CharacterController))]
    //[RequireComponent(typeof(PlayerInput))]

    public class CharacterController : MonoBehaviour
    {        
        private Animator _animator;
        private CharacterController _controller;
        private PlayerInputActions _playerControls;        
        public Rigidbody rb;

        private InputAction move;
        private InputAction jump;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;
        public Vector2 moveDirection = Vector2.zero;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        //player
        private float _speed;        
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        //timeout deltatime

         private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        //animation IDs

         private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall; 

        private bool _hasAnimator;

        private void Awake()
        {
            _playerControls = new PlayerInputActions();
        }

        private void OnEnable()
        {
            move = _playerControls.Player.Move;
            move.Enable();


            jump = _playerControls.Player.Jump;
            jump.performed+= DoJump;
            jump.Enable();
        }
        private void OnDisable()
        {
            move.Disable();
            jump.Disable();
        }

        private void Update()
        {
            moveDirection = move.ReadValue<Vector2>();
            Move();
        }

        private void DoJump(InputAction.CallbackContext obj)
        {
            Debug.Log("Jumped!");
        }

        //private void Start()
        //{
        //    _hasAnimator = TryGetComponent(out _animator);
        //    _controller = GetComponent<CharacterController>();
        //    _input = GetComponent<PlayerInput>();

        //    AssignAnimationIDs();

        //    //reset our timeouts on start
        //    _jumpTimeoutDelta = JumpTimeout;
        //    _fallTimeoutDelta = FallTimeout;
        //}

        //private void Update()
        //{
        //    _hasAnimator = TryGetComponent(out _animator);

        //    JumpAndGravity();
        //    GroundedCheck();
        //    Move();
        //    Actions();
        //}

        //private void AssignAnimationIDs()
        //{
        //    _animIDSpeed = Animator.StringToHash("Speed");
        //    _animIDGrounded = Animator.StringToHash("Grounded");
        //    _animIDJump = Animator.StringToHash("Jump");
        //    _animIDFreeFall = Animator.StringToHash("FreeFall");            
        //}

        //private void GroundedCheck()
        //{
        //    // set sphere position, with offset
        //    Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        //    Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        //    // update animator if using character
        //    if (_hasAnimator)
        //    {
        //        _animator.SetBool(_animIDGrounded, Grounded);
        //    }
        //}


        #region Character Movement

        private void Move()
        {
            _speed = MoveSpeed;

            //move the player              
            rb.velocity = new Vector2(moveDirection.x * _speed , moveDirection.y * _speed);
            
        }
        #endregion

        //#region Actions of Player

        //private void Actions()
        //{

        //}
        //#endregion

        //#region Jump and Gravity

        //private void JumpAndGravity()
        //{
        //    if (Grounded)
        //    {
        //        // reset the fall timeout timer
        //        _fallTimeoutDelta = FallTimeout;

        //        // update animator if using character
        //        if (_hasAnimator)
        //        {
        //            _animator.SetBool(_animIDJump, false);
        //            _animator.SetBool(_animIDFreeFall, false);
        //        }

        //        // stop our velocity dropping infinitely when grounded
        //        if (_verticalVelocity < 0.0f)
        //        {
        //            _verticalVelocity = -2f;
        //        }

        //        // Jump
        //        if (_input.Player.Jump.IsPressed() && _jumpTimeoutDelta <= 0.0f)
        //        {
        //            // the square root of H * -2 * G = how much velocity needed to reach desired height
        //            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

        //            // update animator if using character
        //            if (_hasAnimator)
        //            {
        //                _animator.SetBool(_animIDJump, true);
        //            }
        //        }

        //        // jump timeout
        //        if (_jumpTimeoutDelta >= 0.0f)
        //        {
        //            _jumpTimeoutDelta -= Time.deltaTime;
        //        }
        //    }
        //    else
        //    {
        //        // reset the jump timeout timer
        //        _jumpTimeoutDelta = JumpTimeout;

        //        // fall timeout
        //        if (_fallTimeoutDelta >= 0.0f)
        //        {
        //            _fallTimeoutDelta -= Time.deltaTime;
        //        }
        //        else
        //        {
        //            // update animator if using character
        //            if (_hasAnimator)
        //            {
        //                _animator.SetBool(_animIDFreeFall, true);
        //            }
        //        }                
        //    }

        //    // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        //    if (_verticalVelocity < _terminalVelocity)
        //    {
        //        _verticalVelocity += Gravity * Time.deltaTime;
        //    }
        //}

        //#endregion

    }
}
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
        public Animator _animator;
        public Rigidbody rb;
        private CharacterController _controller;
        private PlayerInputActions _playerControls;          

        private InputAction move;
        private InputAction jump;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;
        public Vector2 moveDirection;

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

        //Animation IDs
        private int _animIDMove;

        //timeout deltatime

         private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;        

        private void Awake()
        {
            _playerControls = new PlayerInputActions();            
            AssignAnimationIDs();            
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
            Move();
                       
        }

        #region Character Movement

        private void Move()
        {
            moveDirection = move.ReadValue<Vector2>();
            _speed = MoveSpeed;
            var newVector= new Vector2(moveDirection.x * _speed, moveDirection.y * _speed);
            
            CheckCharacterFlip();

            //move the player              
            rb.velocity = newVector;
            _animator.SetBool(_animIDMove, (moveDirection.x != 0));
        }
        #endregion

        private void CheckCharacterFlip()
        {
            var charDir = transform.localScale;
            if (moveDirection.x > 0) charDir.x = Mathf.Abs(charDir.x);
            if (moveDirection.x < 0) charDir.x = -1 * Mathf.Abs(charDir.x);
            
            transform.localScale = charDir;
        }

        private void DoJump(InputAction.CallbackContext obj)
        {
            Debug.Log("Jumped!");
        }

        private void AssignAnimationIDs()
        {
            _animIDMove = Animator.StringToHash("IsRunning");
        }

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
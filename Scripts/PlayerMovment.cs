using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
   
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField]
    private float _gravityMultiplier;
    [SerializeField]
    private float _jumpHorizontalSpeed;
    [SerializeField] private float _jumpButtonGracePeriod;
    [SerializeField]
    private Transform _cameraTransform;
    private float originalStepOffset;

    private Animator _animator;
    private CharacterController _charachterController;
    private float ySpeed;
    private float? _lastGroundedTime;
    private float? _jumpButtonPressedTime;
    private bool _isJumping;
    private bool _isGrounded;
    private bool _isSliding;
    private Vector3 _slopeSlideVelocity;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _charachterController = GetComponent<CharacterController>();
        originalStepOffset = _charachterController.stepOffset;
    }

   
    void Update()
    {
      
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movmentDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movmentDirection.magnitude);
        if (Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }
        _animator.SetFloat("InputMagnitude", inputMagnitude,0.05f, Time.deltaTime);
       
        movmentDirection = Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * movmentDirection;
        movmentDirection.Normalize();

        float gravity = Physics.gravity.y * _gravityMultiplier;
        if (_isJumping && ySpeed > 0 && Input.GetButton("Jump" ) == false)
        {
            gravity *= 2;
        }
        ySpeed += gravity * Time.deltaTime;
        SetSlopeSlideVelocity();
        if (_slopeSlideVelocity == Vector3.zero)
        {
            _isSliding = false;
        }

        if (_charachterController.isGrounded)
        {
            _lastGroundedTime = Time.time;
        }
        if (Input.GetButtonDown("Jump"))
        {
            _jumpButtonPressedTime = Time.time;
        }
        if (Time.time - _lastGroundedTime <= _jumpButtonGracePeriod)
        {
            if (_slopeSlideVelocity != Vector3.zero)
            {
                _isSliding = true;
            }
            _charachterController.stepOffset = originalStepOffset;
            if (_isSliding == false)
            { 
                ySpeed = -0.5f;

            }
           
            _animator.SetBool("IsGrounded", true);
            _isGrounded = true;
            _animator.SetBool("IsJumping", false);
            _isJumping = false;
            _animator.SetBool("IsFalling", false);
            if (Time.time - _jumpButtonPressedTime<= _jumpButtonGracePeriod && _isSliding == false)
            {
                ySpeed = Mathf.Sqrt(_jumpHeight * -3 * gravity);
                _animator.SetBool("IsJumping", true);
                _isJumping = true;
                _jumpButtonPressedTime = null;
                _lastGroundedTime = null;
            }
        }
        else
        {
            _charachterController.stepOffset = 0;
            _animator.SetBool("IsGrounded", false);
            _isGrounded = false;
            if ((_isJumping && ySpeed < 0)|| ySpeed < -2)
            {
                _animator.SetBool("IsFalling", true);
            }
        }
       

       
        if (movmentDirection != Vector3.zero)
        {
            _animator.SetBool("IsMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movmentDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
        if (_isGrounded == false && _isSliding == false)
        {
            Vector3 velocity = movmentDirection * inputMagnitude * _jumpHorizontalSpeed;
            velocity.y = ySpeed;
            _charachterController.Move(velocity * Time.deltaTime);
        }
        if (_isSliding)
        {
            Vector3 velocity = _slopeSlideVelocity;
            velocity.y = ySpeed;
            _charachterController.Move(velocity * Time.deltaTime);
        }
        
       
    }
    private void SetSlopeSlideVelocity()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 5))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);
            if (angle >= _charachterController.slopeLimit)
            {
                _slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, ySpeed, 0), hitInfo.normal);
                return;
            }
        }
        if (_isSliding)
        {
            _slopeSlideVelocity -= _slopeSlideVelocity * Time.deltaTime * 3;
            if (_slopeSlideVelocity.magnitude > 1)
            {
                return;
            }
        }
        _slopeSlideVelocity = Vector3.zero;
    }
    private void OnAnimatorMove()
    {
        if (_isGrounded && _isSliding == false)
        {
            Vector3 velocity = _animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime;
            _charachterController.Move(velocity);
        }
       
    }
   
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

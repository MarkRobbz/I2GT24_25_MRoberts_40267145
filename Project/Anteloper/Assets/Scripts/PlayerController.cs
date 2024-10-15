using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _playerController;

    [Header("Movement Speed")] 
    [SerializeField] private float _walkSpeed = 3.0f;
    [SerializeField] private float _sprintMultiplier = 2.0f;

    [Header("Jumping")] 
    [SerializeField] private float _gravity = 9.18f; //Earths Gravity
    [SerializeField] private float _jumpingForce = 6.0f;
    
    [Header("Look Sensitivity")] 
    [SerializeField] private float _mouseSensitivity = 2.0f;
    [SerializeField] private float _lookUpAndDownRange = 80.0f;
    
    [Header("Custom Inputs")] 
    [SerializeField] private String _horizontalMovementInput = "Horizontal";
    [SerializeField] private String _verticalMovementInput = "Vertical";
    [SerializeField] private String _lookRotaionXInput = "Mouse X";
    [SerializeField] private String _lookRotaionYInput = "Mouse Y";
    [SerializeField] private KeyCode _sprintKeyInput = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    
    
    private float _verticalRotation;
    private Vector3 _currentMovement = Vector3.zero;
    private Camera _mainCamera;
    
   
    
    void Start()
    {
        _playerController = gameObject.GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        HandleMovement();
        HandleLookRotation();
        HandleJumpAndGravity();
    }

    void HandleMovement()
    {
        float speedMultiplier = Input.GetKey(_sprintKeyInput) ? _sprintMultiplier : 1f;
        float verticalInput = Input.GetAxis(_verticalMovementInput);
        float horizontalInput = Input.GetAxis(_horizontalMovementInput);
        
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);

        // Normalize the movement vector (to prevent diagonal speed boost)
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        
        movement *= _walkSpeed * speedMultiplier;
        
        movement = transform.rotation * movement;

        
        _currentMovement.x = movement.x;
        _currentMovement.z = movement.z;
        
        
        
        _playerController.Move(_currentMovement * Time.deltaTime);
    }
    

    void HandleLookRotation()
    {
        float xRotation = Input.GetAxis(_lookRotaionXInput) * _mouseSensitivity ;
        transform.Rotate(0,xRotation,0);
        
        _verticalRotation -= Input.GetAxis(_lookRotaionYInput) * _mouseSensitivity ;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_lookUpAndDownRange, _lookUpAndDownRange); //Clamp looking up and down
        _mainCamera.transform.localRotation = Quaternion.Euler(_verticalRotation,0,0);
    }

   

    void HandleJumpAndGravity()
    {
        if (_playerController.isGrounded)
        {
            _currentMovement.y = -0.5f; //Resets player velocity to keep them grounded
            
            if (Input.GetKeyDown(_jumpKey))
            {
                _currentMovement.y = _jumpingForce;
            }
        }
        else
        {
            _currentMovement.y -= _gravity * Time.deltaTime; //Applies gravity
        }
    }

    
}
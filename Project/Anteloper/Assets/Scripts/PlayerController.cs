using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _playerController;

    [Header("Movement Speed")] 
    [SerializeField] private float _walkSpeed = 3.0f;

    [Header("Look Sensitivity")] 
    [SerializeField] private float _mouseSensitivity = 2.0f;
    [SerializeField] private float _lookUpandDownRange = 80.0f;
    
    [Header("Custom Inputs")] 
    [SerializeField] private String _horizontalMovementInput = "Horizontal";
    [SerializeField] private String _verticalMovementInput = "Vertical";
    [SerializeField] private String _lookRotaionXInput = "Mouse X";
    [SerializeField] private String _lookRotaionYInput = "Mouse Y";
    
    private float _verticalRotation;
    private Camera mainCamera;
    
    void Start()
    {
        _playerController = gameObject.GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        HandleMovement();
        HandleLookRotation();
    }

    void HandleMovement()
    {
        float verticalInput = Input.GetAxis(_verticalMovementInput);
        float horizontalInput = Input.GetAxis(_horizontalMovementInput);

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        
        // Normalise movement vector if magnitude is greater than 1
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        
        movement = transform.rotation * movement;
        
        Vector3 speed = movement * _walkSpeed;
        
        _playerController.SimpleMove(speed);
    }

    void HandleLookRotation()
    {
        float xRotation = Input.GetAxis(_lookRotaionXInput) * _mouseSensitivity ;
        transform.Rotate(0,xRotation,0);
        
        _verticalRotation -= Input.GetAxis(_lookRotaionYInput) * _mouseSensitivity ;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_lookUpandDownRange, _lookUpandDownRange); //Clamp looking up and down
        mainCamera.transform.localRotation = Quaternion.Euler(_verticalRotation,0,0);
    }
}
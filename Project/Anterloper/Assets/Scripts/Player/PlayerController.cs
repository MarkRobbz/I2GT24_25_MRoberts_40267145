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
    [SerializeField] private float _gravity = 9.18f; // Earth's Gravity
    [SerializeField] private float _jumpingForce = 6.0f;
    
    [Header("Look Sensitivity")] 
    [SerializeField] private float _mouseSensitivity = 2.0f;
    [SerializeField] private float _lookUpAndDownRange = 80.0f;
    
    [Header("Custom Inputs")] 
    [SerializeField] private String _horizontalMovementInput = "Horizontal";
    [SerializeField] private String _verticalMovementInput = "Vertical";
    [SerializeField] private String _lookRotationXInput = "Mouse X";
    [SerializeField] private String _lookRotationYInput = "Mouse Y";
    [SerializeField] private KeyCode _sprintKeyInput = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _equipmentAction1 = KeyCode.Mouse0;
    
    private float _verticalRotation;
    private Vector3 _currentMovement = Vector3.zero;
    private Camera _mainCamera;
    
    private PlayerEquipment _playerEquipment;

    private InventoryUI _inventoryUI;
    private CraftingUI _craftingUI;
    
    void Start()
    {
        _playerController = gameObject.GetComponent<CharacterController>();
        _playerEquipment = gameObject.GetComponent<PlayerEquipment>();
        _mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        _craftingUI = GameObject.FindObjectOfType<CraftingUI>();
    }
    
    void Update()
    {
        HandleEquipmentAction();
        HandleMovement();
        HandleJumpAndGravity(); 
        
        if (_inventoryUI.IsInventoryUIActive() || _craftingUI.IsCraftingUIActive())
        {
            //Doesn't let look about if UI is open
            return;
        }
        
        HandleLookRotation();
    }

   
    void HandleMovement()
    {
        float speedMultiplier = Input.GetKey(_sprintKeyInput) ? _sprintMultiplier : 1f;
        float verticalInput = Input.GetAxis(_verticalMovementInput);
        float horizontalInput = Input.GetAxis(_horizontalMovementInput);

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        
        // Normalize movement vector if magnitude is greater than 1 (diagonal movement)
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        
        
        movement *= _walkSpeed * speedMultiplier;
        
        movement = transform.rotation * movement;
        
        // Update horizontal movement
        _currentMovement.x = movement.x;
        _currentMovement.z = movement.z;
        
        
        _playerController.Move(_currentMovement * Time.deltaTime);
    }

    void HandleLookRotation()
    {
        float xRotation = Input.GetAxis(_lookRotationXInput) * _mouseSensitivity;
        transform.Rotate(0, xRotation, 0); 
        
        _verticalRotation -= Input.GetAxis(_lookRotationYInput) * _mouseSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_lookUpAndDownRange, _lookUpAndDownRange); // Clamp looking up and down
        
        _mainCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0); 
    }

    void HandleJumpAndGravity()
    {
   
        if (_playerController.isGrounded)
        {
            _currentMovement.y = -0.5f; // Keep grounded
            if (Input.GetKeyDown(_jumpKey))
            {
                _currentMovement.y = _jumpingForce; 
            }
        }
        else
        {
            // Apply gravity while in the airs
            _currentMovement.y -= _gravity * Time.deltaTime;
        }
    }

    void HandleEquipmentAction()
    {
        if (Input.GetKeyDown(_equipmentAction1)) 
        {
            if (_playerEquipment.equippedItem != null)
            {
                if (_playerEquipment.equippedItem is ToolItem toolItem)
                {
                    toolItem.UseTool();
                }
                else
                {
                    Debug.Log($"{_playerEquipment.equippedItem.itemName} cannot be used.");
                }
            }
        }
    }


    
    
}

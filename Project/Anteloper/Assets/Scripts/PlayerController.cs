using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _playerController;

    [Header("Movement Speed")] 
    [SerializeField] private float _walkSpeed = 2.0f;

    [Header("Custom Inputs")] 
    [SerializeField] private String _horizontalMovementInput = "Horizontal";
    [SerializeField] private String _verticalMovementInput = "Vertical";
    void Start()
    {
        _playerController = gameObject.GetComponent<CharacterController>();
    }
    
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float verticalSpeed = Input.GetAxis(_verticalMovementInput) * _walkSpeed;
        float horizontalSpeed = Input.GetAxis(_horizontalMovementInput) * _walkSpeed;

        Vector3 speed = new Vector3(horizontalSpeed, 0, verticalSpeed);
        speed = transform.rotation * speed;
        
        _playerController.SimpleMove(speed);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int _speed = 20;
    private float _turnSpeed = 10f;
    private float _horizontalInput;
    private float _verticalInput;
    
    private void FixedUpdate()
    {
        DriveFoward();
        DriveTurning();
    }

    private void DriveFoward()
    {
                //Move car forward  on Z axis
            _verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.forward * Time.deltaTime * _speed * _verticalInput);
            
        
    }

    private void DriveTurning()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * Time.deltaTime * _turnSpeed * _horizontalInput);
    }
}

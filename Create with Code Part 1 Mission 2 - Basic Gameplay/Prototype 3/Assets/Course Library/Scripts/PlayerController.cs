using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _gravityModifier;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool isGrounded = true;
    
    [Header("Keybindings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
   
    void Start()
    {
        Physics.gravity *= _gravityModifier;
        _rb = gameObject.GetComponent<Rigidbody>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(_jumpKey) && isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

   
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5.0f;
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private GameObject _focalPoint;

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        float fowardInput = Input.GetAxis("Vertical");
        
        _playerRb.AddForce(_focalPoint.transform.forward * Speed * fowardInput);
    }
}

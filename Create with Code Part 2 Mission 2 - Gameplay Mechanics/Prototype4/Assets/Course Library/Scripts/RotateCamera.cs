using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _rotationSpeed = 10.0f;
    
    
    
    void Start()
    {
        _mainCamera = GameObject.FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up,horizontalInput * _rotationSpeed * Time.deltaTime);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _rayDistance = 3.0f;
    [SerializeField] private LayerMask _interactableLayer =  1 << 8;
    [SerializeField] private KeyCode _interactKey = KeyCode.E;

    private void Start()
    {
        _playerCamera = gameObject.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_interactKey))
        {
            TryInteract(); 
        }
    }


    void TryInteract()
    {
        RaycastHit hit;
        
        Debug.DrawRay(_playerCamera.transform.position, _playerCamera.transform.forward * _rayDistance, Color.red, 1.0f);
        
        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _rayDistance, _interactableLayer))
        {
            IUsable usable = hit.collider.GetComponent<IUsable>();

            if (usable != null)
            {
                usable.Use(); // Call the Use() on interactable object
            }
        }
}
}

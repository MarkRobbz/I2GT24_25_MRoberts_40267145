using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _rayCastDistance = 3.0f;
    [SerializeField] private LayerMask _interactableLayer = 1 << 8;  
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private TextMeshProUGUI interactionUI; 

    private IUsable _currentUsable;  

    private void Start()
    {
        // Get the player camera and the interaction UI element
        _playerCamera = gameObject.GetComponentInChildren<Camera>();
        interactionUI = GameObject.FindGameObjectWithTag("InteractPromptUI").GetComponentInChildren<TextMeshProUGUI>();

        // Initially hide the interaction prompt
        interactionUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        DetectInteractableObject();
        
        if (Input.GetKeyDown(_interactKey) && _currentUsable != null)
        {
            _currentUsable.Use(); 
        }
    }

    void DetectInteractableObject()
    {
        RaycastHit hit;
        
   
        Debug.DrawRay(_playerCamera.transform.position, _playerCamera.transform.forward * _rayCastDistance, Color.red, 0.5f);

        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _rayCastDistance, _interactableLayer))
        {
            IUsable usable = hit.collider.GetComponent<IUsable>();

            if (usable != null)
            {
                _currentUsable = usable;
                interactionUI.text = "Press E to Pickup";  
                interactionUI.gameObject.SetActive(true);  
            }
        }
        else
        {
            _currentUsable = null;
            interactionUI.gameObject.SetActive(false);  
        }
    }
}
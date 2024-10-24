using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _raycastDistance = 3.0f;
    [SerializeField] private float _sphereRadius = 0.5f;
    [SerializeField] private LayerMask _interactableLayer = 1 << 8;
    [SerializeField] private TextMeshProUGUI _interactionUI;
    
    
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private float _holdThreshold = 0.5f; // Time in seconds considered as hold
    private float _interactKeyHoldTime = 0f;

    private IUsable _currentUsable;
    private int _frameCount = 0;
    private int _checkInterval = 10;

    private bool showRaycast = false;  

    private void Start()
    {
        if (_playerCamera == null)
        {
            _playerCamera = gameObject.GetComponentInChildren<Camera>();
        }
        
        _interactionUI = GameObject.FindGameObjectWithTag("InteractPromptUI").GetComponentInChildren<TextMeshProUGUI>();
        _interactionUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        _frameCount++;

        if (_frameCount % _checkInterval == 0)
        {
            DetectInteractableObject();
        }

        if (Input.GetKeyDown(_interactKey))
        {
            _interactKeyHoldTime = 0f;
            Debug.Log("Key Down detected");
        }
        if (Input.GetKey(_interactKey))
        {
            _interactKeyHoldTime += Time.deltaTime;
            Debug.Log("Key Held, Hold Time: " + _interactKeyHoldTime);
        }
        if (Input.GetKeyUp(_interactKey))
        {
            bool isHold = _interactKeyHoldTime >= _holdThreshold;
            Debug.Log("Key Up detected, isHold: " + isHold);
            _currentUsable.Use(isHold);
            _interactKeyHoldTime = 0f;
        }
    }
    
    void DetectInteractableObject()
    {
        RaycastHit hit;
        Vector3 origin = _playerCamera.transform.position;
        Vector3 direction = _playerCamera.transform.forward;

        bool hitDetected = Physics.SphereCast(origin, _sphereRadius, direction, out hit, _raycastDistance, _interactableLayer);

        if (hitDetected)
        {
            IUsable usable = hit.collider.GetComponent<IUsable>();

            if (usable != null)
            {
                _currentUsable = usable;
                if (usable is ItemPickup itemPickup)
                {
                    if (itemPickup.item is ConsumableItem)
                    {
                        _interactionUI.text = "E to Pickup, Hold to Consume";
                    }
                    else
                    {
                        _interactionUI.text = "Press E to Pickup";
                    }
                }
                else
                {
                    _interactionUI.text = "Press E to Interact";
                }
                _interactionUI.gameObject.SetActive(true);
            }
        }
        else
        {
            _currentUsable = null;
            _interactionUI.gameObject.SetActive(false);
        }
    }

    
    
    
    
    
            
            //*****EDITOR DEBUG TOOLS****//
    
    [Button("Toggle Raycast Visualisation")]
    public void ToggleRaycastVisualisation()
    {
        showRaycast = !showRaycast;
    }
    
    private void OnDrawGizmos()
    {
        if (showRaycast)
        {
            Vector3 origin = _playerCamera.transform.position;
            Vector3 direction = _playerCamera.transform.forward;

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(origin, direction * _raycastDistance);

            RaycastHit hit;
            if (Physics.SphereCast(origin, _sphereRadius, direction, out hit, _raycastDistance, _interactableLayer))
            {
                
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(hit.point, _sphereRadius);
            }
            else
            {
                Gizmos.DrawWireSphere(origin + direction * _raycastDistance, _sphereRadius);
            }
        }
    }
}
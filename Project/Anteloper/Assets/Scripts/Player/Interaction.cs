using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _raycastDistance = 3.0f;
    [SerializeField] private float _sphereRadius = 0.5f;
    [SerializeField] private LayerMask _interactableLayer = 1 << 8;
    [SerializeField] private LayerMask _pickupLayer = 1 << 9;
    [SerializeField] private TextMeshProUGUI _interactionUI;
    
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private float _holdThreshold = 0.5f; //seconds required to consume
    private float _interactKeyHoldTime = 0f;
    private bool _consumed = false;

    [SerializeField] private Inventory _inventory;

    private IUsable _currentUsable;
    private int _frameCount = 0;
    private int _checkInterval = 10;

    private bool _showRaycast = false;  

    private void Start()
    {
        if (_playerCamera == null)
        {
            _playerCamera = gameObject.GetComponentInChildren<Camera>();
        }
        
        _interactionUI = GameObject.FindGameObjectWithTag("InteractPromptUI").GetComponentInChildren<TextMeshProUGUI>();
        _interactionUI.gameObject.SetActive(false);
        _inventory = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        _frameCount++;
        
        if (_frameCount % _checkInterval == 0)
        {
            DetectInteractableObject();
        }
        
        HandleInteractionInput();
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(_interactKey))
        {
            StartHold();
        }

        if (Input.GetKey(_interactKey))
        {
            UpdateHold();
        }

        if (Input.GetKeyUp(_interactKey))
        {
            EndHold();
        }
    }

    private void StartHold()
    {
        _interactKeyHoldTime = 0f;
        _consumed = false;
    }

    private void UpdateHold()
    {
        _interactKeyHoldTime += Time.deltaTime;

        if (_interactKeyHoldTime >= _holdThreshold && !_consumed)
        {
            _currentUsable?.Use(true);
            _consumed = true;
        }
    }

    private void EndHold()
    {
        if (!_consumed)
        {
            _currentUsable?.Use(false);
        }
        _interactKeyHoldTime = 0f;
        _consumed = false;
    }
    
  private void DetectInteractableObject()
{
    RaycastHit hit;
    Vector3 origin = _playerCamera.transform.position;
    Vector3 direction = _playerCamera.transform.forward;

    bool hitDetected = Physics.SphereCast(origin, _sphereRadius, direction, out hit, _raycastDistance, _interactableLayer | _pickupLayer);

    if (hitDetected)
    {
       // Debug.Log($"Hit detected on object: {hit.collider.gameObject.name}");
        IUsable usable = hit.collider.GetComponentInParent<IUsable>();
        
        /*if (usable != null)
        {
            Debug.Log($"Usable component found: {usable.GetType().Name} on object {((MonoBehaviour)usable).gameObject.name}");
        }
        else
        {
            Debug.Log("No IUsable component found on the hit object or its parents.");
        }*/

        if (usable != null)
        {
            _currentUsable = usable;
            
            if (((MonoBehaviour)usable).gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                _interactionUI.text = "Press E to Interact";
            }
            else if (((MonoBehaviour)usable).gameObject.layer == LayerMask.NameToLayer("Pickups"))
            {
                if (_inventory.IsInventoryFull())
                {
                    _interactionUI.text = "Inventory Full";
                }
                else if (usable is ItemPickup itemPickup)
                {
                    //Debug.Log($"Item Type: {itemPickup.item.GetType()}");
                    //Debug.Log($"Is EdibleItem: {itemPickup.item is EdibleItem}");
                    //Debug.Log($"Is ConsumableItem: {itemPickup.item is ConsumableItem}");
                    //Debug.Log($"Is BaseItem: {itemPickup.item is BaseItem}");

                    if (itemPickup.item is EdibleItem)
                    {
                        _interactionUI.text = "Press E to Pickup, Hold E to Consume";
                    }
                    else if (itemPickup.item is ConsumableItem)
                    {
                        _interactionUI.text = "Press E to Pickup, Hold E to Consume";
                    }
                    else if (itemPickup.item is CraftableItem)
                    {
                        _interactionUI.text = "Press E to Pickup";
                    }
                    else if (itemPickup.item is BaseItem)
                    {
                        _interactionUI.text = "Press E to Pickup";
                    }
                }
                else
                {
                    _interactionUI.text = "Press E to Pickup";
                }
            }
            else
            {
                Debug.Log($"Usable object {((MonoBehaviour)usable).gameObject.name} is on layer {LayerMask.LayerToName(((MonoBehaviour)usable).gameObject.layer)} which is not handled.");
            }
            _interactionUI.gameObject.SetActive(true);
        }
        else
        {
            _currentUsable = null;
            _interactionUI.gameObject.SetActive(false);
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
        _showRaycast = !_showRaycast;
    }
    
    private void OnDrawGizmos()
    {
        if (_showRaycast)
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

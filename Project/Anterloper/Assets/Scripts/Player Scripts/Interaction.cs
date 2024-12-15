using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using System.Collections;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _raycastDistance = 3.0f;
    [SerializeField] private float _sphereRadius = 0.5f;
    [SerializeField] private LayerMask _interactableLayer = 1 << 8;
    [SerializeField] private LayerMask _pickupLayer = 1 << 9;
    [SerializeField] private LayerMask _nodeLayer = 1 << 10;
    [SerializeField] private LayerMask _excludedLayers;
    [SerializeField] private TextMeshProUGUI _interactionUI;

    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private float _holdThreshold = 0.5f; //seconds required to consume
    private float _interactKeyHoldTime = 0f;
    private bool _consumed = false;

    [SerializeField] private Inventory _inventory;

    private IUsable _currentUsable;
    private bool _showRaycast = false;

    private GameObject _currentInteractableObject;
    private Transform _playerCameraTransform;

    private void Start()
    {
        if (_playerCamera == null)
        {
            _playerCamera = gameObject.GetComponentInChildren<Camera>();
        }

        _playerCameraTransform = _playerCamera.transform;

        _interactionUI = GameObject.FindGameObjectWithTag("InteractPromptUI").GetComponentInChildren<TextMeshProUGUI>();
        _interactionUI.gameObject.SetActive(false);
        _inventory = FindObjectOfType<Inventory>();

        int equippedItemLayer = LayerMask.NameToLayer("EquippedItem");
        _excludedLayers = 1 << equippedItemLayer;

        StartCoroutine(DetectInteractableObjectRoutine());
    }

    private void Update()
    {
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

    private IEnumerator DetectInteractableObjectRoutine()
    {
        while (true)
        {
            DetectInteractableObject();
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }
    }

    private void DetectInteractableObject()
    {
        RaycastHit hit;
        Vector3 origin = _playerCameraTransform.position;
        Vector3 direction = _playerCameraTransform.forward;

        int layerMask = (_interactableLayer | _pickupLayer | _nodeLayer) & ~_excludedLayers;

        bool hitDetected = Physics.SphereCast(origin, _sphereRadius, direction, out hit, _raycastDistance, layerMask);

        if (hitDetected)
        {
            IUsable usable = hit.collider.GetComponentInParent<IUsable>();

            if (usable != null)
            {
                _currentUsable = usable;
                _currentInteractableObject = ((MonoBehaviour)usable).gameObject;

                if (_currentInteractableObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    _interactionUI.text = "Press E to Interact";
                }
                else if (_currentInteractableObject.layer == LayerMask.NameToLayer("Pickups"))
                {
                    if (_inventory.IsInventoryFull())
                    {
                        _interactionUI.text = "Inventory Full";
                    }
                    else if (usable is ItemPickup itemPickup)
                    {
                        if (itemPickup.item is EdibleItem || itemPickup.item is ConsumableItem)
                        {
                            _interactionUI.text = "Press E to Pickup, Hold E to Consume";
                        }
                        else
                        {
                            _interactionUI.text = "Press E to Pickup";
                        }
                    }
                    else
                    {
                        _interactionUI.text = "Press E to Pickup";
                    }
                }
                _interactionUI.gameObject.SetActive(true);
            }
            else
            {
                Tree tree = hit.collider.GetComponentInParent<Tree>();
                if (tree != null)
                {
                    _currentUsable = null;
                    _currentInteractableObject = tree.gameObject;
                    _interactionUI.text = "Use Axe to Chop Tree";
                    _interactionUI.gameObject.SetActive(true);
                }
                else
                {
                    _currentUsable = null;
                    _currentInteractableObject = null;
                    _interactionUI.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            _currentUsable = null;
            _currentInteractableObject = null;
            _interactionUI.gameObject.SetActive(false);
        }
    }

    public GameObject GetCurrentInteractableObject()
    {
        if (_currentUsable != null)
        {
            return ((MonoBehaviour)_currentUsable).gameObject;
        }
        else
        {
            RaycastHit hit;
            Vector3 origin = _playerCameraTransform.position;
            Vector3 direction = _playerCameraTransform.forward;

            int layerMask = Physics.DefaultRaycastLayers & ~_excludedLayers;

            bool hitDetected = Physics.SphereCast(origin, _sphereRadius, direction, out hit, _raycastDistance, layerMask);

            if (hitDetected)
            {
                Tree tree = hit.collider.GetComponentInParent<Tree>();
                if (tree != null)
                {
                    return tree.gameObject;
                }
            }
        }

        return null;
    }

    [Button("Toggle Raycast Visualisation")]
    public void ToggleRaycastVisualisation()
    {
        _showRaycast = !_showRaycast;
    }

    private void OnDrawGizmos()
    {
        if (_showRaycast)
        {
            Vector3 origin = _playerCameraTransform.position;
            Vector3 direction = _playerCameraTransform.forward;

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
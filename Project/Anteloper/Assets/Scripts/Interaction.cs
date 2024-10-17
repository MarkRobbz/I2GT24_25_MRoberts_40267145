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

        if (Input.GetKeyDown(_interactKey) && _currentUsable != null)
        {
            _currentUsable.Use();
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
                _interactionUI.text = "Press E to Pickup";
                _interactionUI.gameObject.SetActive(true);
            }
        }
        else
        {
            _currentUsable = null;
            _interactionUI.gameObject.SetActive(false);
        }
    }

    
    
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
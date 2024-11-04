using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5.0f;
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private GameObject _focalPoint;
    public bool isPickupActive = false;
    [SerializeField] private float _powerUpStrength = 0.0f;
    [SerializeField] private GameObject _powerUpIndicator;

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
       

    }

    private void Update()
    {
        float fowardInput = Input.GetAxis("Vertical");
        
        _playerRb.AddForce(_focalPoint.transform.forward * Speed * fowardInput);
        _powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        
            if (other.gameObject.CompareTag("Powerup"))
            {
                _powerUpIndicator.gameObject.SetActive(true);
                _powerUpStrength = other.GetComponent<PowerUp>().powerUpStrengh;
                isPickupActive = true;
                Destroy(other.gameObject);
                StartCoroutine(PowerUpContdownRoutine());
            }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && isPickupActive)
        {
            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (transform.position - other.gameObject.transform.position).normalized;
        
            enemyRb.AddForce(awayFromPlayer * _powerUpStrength, ForceMode.Impulse);
        }
    }

    IEnumerator PowerUpContdownRoutine()
    {
        
        yield return new WaitForSeconds(7);
        isPickupActive = false;
        _powerUpIndicator.gameObject.SetActive(false);
    }
}

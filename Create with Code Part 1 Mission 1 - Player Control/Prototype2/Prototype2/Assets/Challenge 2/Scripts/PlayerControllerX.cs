using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;

    private bool canSpawn = true;  

    [SerializeField] private float spawnCooldown = 2.0f;  

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSpawn)
        {
            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
            canSpawn = false;  
            Invoke("EnableSpawning", spawnCooldown);  
        }
    }

    
    private void EnableSpawning()
    {
        canSpawn = true;
    }
}
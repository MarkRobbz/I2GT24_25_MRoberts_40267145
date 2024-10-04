using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float horizontalInput;
    [SerializeField] private float speed = 10f;
    private int xConstraint = 15;

    [SerializeField] private GameObject projectilePrefab;

    void Update()
    {
        PlayerMovement();
        if (transform.position.x < -xConstraint) {
            transform.position = new Vector3(-xConstraint, transform.position.y, transform.position.z);
        } else if(transform.position.x > xConstraint) {
            transform.position = new Vector3(xConstraint, transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(projectilePrefab, new Vector3(transform.position.x,
                transform.position.y,transform.position.z), Quaternion.identity);
        }
        
    }

    private void PlayerMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        } else if (horizontalInput < 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
    }
}

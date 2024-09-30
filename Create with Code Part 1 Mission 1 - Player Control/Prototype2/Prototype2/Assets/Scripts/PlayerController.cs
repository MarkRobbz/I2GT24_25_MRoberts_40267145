using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float horizontalInput;
    [SerializeField] private float speed = 10f;

    
    void Update()
    {
        PlayerMovement();
        if (transform.position.x < -15)
        {
            //transform.position.Set(-15, transform.position.y, transform.position.z);
            transform.position = new Vector3(-15, transform.position.y, transform.position.z);
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

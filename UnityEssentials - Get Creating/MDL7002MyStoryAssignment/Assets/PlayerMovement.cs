using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Animator anim;
    private Rigidbody rb;



    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        Jump();
        Move();
        still();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rb.AddForce(Vector3.up * 10 * Time.deltaTime, ForceMode.Impulse);
            this.anim.SetBool("jump", true);
            //Debug.Log("jump");
        }
        else
        {
            this.anim.SetBool("jump", false);
        }
    }

    private void Move()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        Vector3 movement = this.transform.forward * verticalAxis + this.transform.right * horizontalAxis;
        movement.Normalize();

        this.transform.position += movement * 0.1f;

        this.anim.SetFloat("vertical", verticalAxis);
        this.anim.SetFloat("horizontal", horizontalAxis);
    }

    private void still()
    {
        this.rb.AddForce(Vector3.down * 1 * Time.deltaTime, ForceMode.Impulse);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFoward : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    
    // Update is called once per frame
    void Update()
    {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        
    }
}

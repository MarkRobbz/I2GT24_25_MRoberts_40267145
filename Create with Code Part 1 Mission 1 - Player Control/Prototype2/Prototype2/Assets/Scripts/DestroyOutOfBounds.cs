using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    [SerializeField] private float _destroyGameObjRange = 40f;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z >= _destroyGameObjRange || transform.position.z <= -_destroyGameObjRange)
        {
            Destroy(gameObject);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody _enemyRb;
    [SerializeField] private GameObject _player;
    public float speed = 0.0f;

    private void Start()
    {
        _enemyRb = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
        Vector3 lookDireaction = (_player.transform.position - transform.position).normalized;
        _enemyRb.AddForce(lookDireaction * speed);
    }
}

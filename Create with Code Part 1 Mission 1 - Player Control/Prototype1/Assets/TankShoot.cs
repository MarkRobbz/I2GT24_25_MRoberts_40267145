using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    public Rigidbody bulletPrefab; 
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _bulletSpeed = 60f; 
    [SerializeField] private int _maxBullets = 10;
    private Queue<Rigidbody> bullets = new Queue<Rigidbody>(); 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody bulletInstance = Instantiate(bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            bulletInstance.velocity = _bulletSpawnPoint.forward * _bulletSpeed;
            
            bullets.Enqueue(bulletInstance);
            if (bullets.Count > _maxBullets)
            {
                Rigidbody oldestBullet = bullets.Dequeue();
                Destroy(oldestBullet.gameObject);
            }
        }
    }
}



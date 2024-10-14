using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private float _repeatInterval = 2.0f;
    [SerializeField] private float _startDelay = 2.0f;

    // Update is called once per frame
    void Start()
    {
            
            InvokeRepeating("spawnObstacle",_startDelay,_repeatInterval);
        
    }

    private void spawnObstacle()
    {
        Instantiate(_obstacle, _obstacle.transform.position, Quaternion.identity);
    }
}

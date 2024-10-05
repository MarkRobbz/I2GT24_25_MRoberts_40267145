using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] animalsArr;

    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float spawnDelay = 1.0f;
    [SerializeField] private int spawnRangeX = 16;
    [SerializeField] private int spawnPosZ = 21;

    private void Start()
    {
          InvokeRepeating("SpawnRandomAnimal",spawnDelay, spawnInterval);
                
    }
    

    private void SpawnRandomAnimal()
    {
        GameObject randomAnimal = animalsArr[Random.Range(0, animalsArr.Length)];
        Instantiate(randomAnimal, new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ),randomAnimal.transform.localRotation);

    }

    
}

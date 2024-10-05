using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] animalsArr;

    [SerializeField] private int spawnRangeX = 16;

    [SerializeField] private int spawnPosZ = 21;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject randomAnimal = animalsArr[Random.Range(0, animalsArr.Length)];
            Instantiate(randomAnimal, new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ),randomAnimal.transform.localRotation);
        }
    }
}

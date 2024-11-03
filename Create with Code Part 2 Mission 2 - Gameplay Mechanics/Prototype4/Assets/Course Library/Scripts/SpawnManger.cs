using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManger : MonoBehaviour
{
   public GameObject enemyPrefab;
   [SerializeField] private float _spawnRange = 9;

   private void Start()
   {
    
      Instantiate(enemyPrefab, GenerateSpawnPositions(), enemyPrefab.transform.rotation);
   }

   private Vector3 GenerateSpawnPositions()
   {
      float randomX = UnityEngine.Random.Range(-_spawnRange, _spawnRange);
      float randomZ = UnityEngine.Random.Range(-_spawnRange, _spawnRange);
      Vector3 randomSpawnPos = new Vector3(randomX, 0, randomZ);

      return randomSpawnPos;
   }
}

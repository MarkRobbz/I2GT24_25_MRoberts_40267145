using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManger : MonoBehaviour
{
   public GameObject enemyPrefab;
   [SerializeField] private float _spawnRange = 9;
   public int enemyCount = 0;
   public int waveNumber = 0;
   public GameObject powerupPrefab;

   private void Start()
   {
      Instantiate(powerupPrefab, GenerateSpawnPositions(), powerupPrefab.transform.rotation);
      SpawnEnemyWave(waveNumber);
   }

   private void Update()
   {
      enemyCount = FindObjectsOfType<Enemy>().Length;
      if (enemyCount == 0)
      {
         waveNumber++;
         SpawnEnemyWave(waveNumber);
      }
   }

   private void SpawnEnemyWave(int enemiesToSpawn)
   {
      for (int i = 0; i < enemiesToSpawn; i++)
      {
         Instantiate(enemyPrefab, GenerateSpawnPositions(), enemyPrefab.transform.rotation);
      }
   }

   private Vector3 GenerateSpawnPositions()
   {
      float randomX = UnityEngine.Random.Range(-_spawnRange, _spawnRange);
      float randomZ = UnityEngine.Random.Range(-_spawnRange, _spawnRange);
      Vector3 randomSpawnPos = new Vector3(randomX, 0, randomZ);

      return randomSpawnPos;
   }
}

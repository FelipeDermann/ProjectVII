using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy that will be spawned at each wave")]
    public GameObject[] enemyToSpawn;

    [Header("Control variables. Do not alter these.")]
    public int currentEnemyIndex;
    public bool spawnedEnemyIsAlive;
    public WaveSpawnerController spawnerController;

    void Start()
    {
        spawnerController = transform.parent.GetComponent<WaveSpawnerController>();    
    }

    public void SpawnEnemy()
    {
        GameObject enemySpawned = Instantiate(enemyToSpawn[currentEnemyIndex], transform.position, transform.rotation);
        enemySpawned.GetComponent<Enemy>().myWaveSpawner = this;

        spawnedEnemyIsAlive = true;
    }

    public void SpawnedEnemyDead()
    {
        currentEnemyIndex += 1;

        spawnedEnemyIsAlive = false;

        spawnerController.CheckIfCanSpawn();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy that will be spawned at each wave")]
    public ObjectPool[] enemyToSpawn;

    [Header("Control variables. Do not alter these.")]
    public int currentEnemyPoolIndex;
    public bool spawnedEnemyIsAlive;
    public WaveSpawnerController spawnerController;

    void Start()
    {
        spawnerController = transform.parent.GetComponent<WaveSpawnerController>();    
    }

    public void SpawnEnemy()
    {
        //GameObject enemySpawned = Instantiate(enemyToSpawn[currentEnemyIndex], transform.position, transform.rotation);
        //PoolableObject enemySpawned = GameManager.Instance.EnemyPool.RequestObject(transform.position, transform.rotation);
        PoolableObject enemySpawned = enemyToSpawn[currentEnemyPoolIndex].RequestObject(transform.position, transform.rotation);
        Enemy enemySpawnedScript = enemySpawned.GetComponent<Enemy>();

        enemySpawnedScript.poolToReturnTo = enemyToSpawn[currentEnemyPoolIndex];
        enemySpawnedScript.myWaveSpawner = this;
        enemySpawned.transform.parent = null;

        enemySpawnedScript.Activate();

        spawnedEnemyIsAlive = true;
    }

    public void SpawnedEnemyDead()
    {
        currentEnemyPoolIndex += 1;

        spawnedEnemyIsAlive = false;

        spawnerController.CheckIfCanSpawn();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject[] enemyToSpawn;
    public int currentEnemyIndex;
    public bool spawnedEnemyIsAlive;

    void SpawnEnemy()
    {
        GameObject enemySpawned = Instantiate(enemyToSpawn[currentEnemyIndex], transform.position, transform.rotation);
        enemySpawned.GetComponent<Enemy>().myWaveSpawner = this;

        spawnedEnemyIsAlive = true;
    }

    public void SpawnedEnemyDead()
    {
        currentEnemyIndex += 1;

        spawnedEnemyIsAlive = false;
    }
}

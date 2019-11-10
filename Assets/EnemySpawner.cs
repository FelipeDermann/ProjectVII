using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefabToSpawn;
    public bool readyToSpawn;

    [Header("Time for the spawner to spawn the enemy in seconds")]
    public float timeToSpawn;

    void Start()
    {
        CallSpawnEnemy();
    }

    public void CallSpawnEnemy()
    {
        Invoke(nameof(SpawnEnemy), timeToSpawn);
    }

    void SpawnEnemy()
    {
        GameObject enemySpawn = Instantiate(enemyPrefabToSpawn, transform.position, transform.rotation);
        enemySpawn.GetComponent<Enemy>().mySpawner = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualSpawner : MonoBehaviour
{
    public GameObject enemyPrefabToSpawn;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3)) SpawnEnemy();    
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefabToSpawn, transform.position, transform.rotation);
    }
}


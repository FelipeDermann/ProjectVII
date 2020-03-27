﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerController : MonoBehaviour
{
    [Header("Set up variables. Alter these at will")]
    public float numberOfWaves;
    public float waitTimeBetweenWaves;
    public GameObject barrierToDeactivate;

    [Header("Control variables. Do not alter these.")]
    public List<WaveSpawner> spawners;
    float currentNumberOfWaves;
    public bool playerInRange;
    public bool waveCompleted;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<WaveSpawner>() != null)
                spawners.Add(transform.GetChild(i).GetComponent<WaveSpawner>());
        }
    }

    public void IsPlayerInRange(bool _state)
    {
        playerInRange = _state;

        if (_state)
        {
            CheckIfCanSpawn();
        }
    }

    public void CheckIfCanSpawn()
    {
        if (waveCompleted) return;

        float enemiesAlive = 0;

        foreach (WaveSpawner waveSpawner in spawners)
        {
            if (waveSpawner.spawnedEnemyIsAlive) enemiesAlive += 1;
        }

        if (enemiesAlive <= 0)
        {
            if (currentNumberOfWaves == numberOfWaves)
            {
                waveCompleted = true;
                DisableBarrier();
                return;
            }
           
            else Invoke(nameof(SpawnWaveOfEnemies), waitTimeBetweenWaves);
        }
        else Debug.Log("ENEMIES ARE STILL ALIVE");

    }

    void DisableBarrier()
    {
        barrierToDeactivate.SetActive(false);
    }

    void SpawnWaveOfEnemies()
    {
        Debug.Log("WAVE OF ENEMIES BEING SPAWNED");

        foreach (WaveSpawner waveSpawner in spawners)
        {
            waveSpawner.SpawnEnemy();
        }
        currentNumberOfWaves += 1;
    }
}
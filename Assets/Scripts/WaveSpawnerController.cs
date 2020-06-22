using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveSpawnerController : MonoBehaviour
{
    public static event Action FinalWaveEnd;

    [Header("Set up variables. Alter these at will")]
    public float numberOfWaves;
    public float waitTimeBetweenWaves;
    public GameObject[] barriers;

    [Header("Control variables. Do not alter these.")]
    public List<WaveSpawner> spawners;
    float currentNumberOfWaves;
    public bool startWaveOfEnemies;
    public bool waveCompleted;
    public bool finalWave;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<WaveSpawner>() != null)
                spawners.Add(transform.GetChild(i).GetComponent<WaveSpawner>());
        }
    }

    public void IsPlayerInRange()
    {
        startWaveOfEnemies = true;

        CheckIfCanSpawn();
        EnableBarriers();
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
                DisableBarriers();
                return;
            }

            else
            {
                CancelInvoke();
                Invoke(nameof(SpawnWaveOfEnemies), waitTimeBetweenWaves);
            }
        }

    }

    void DisableBarriers()
    {
        Music.Instance.StartMusicFade(false);
        
        for (int i = 0; i < barriers.Length; i++)
            barriers[i].SetActive(false);

        if (finalWave) FinalWaveEnd?.Invoke();
    }

    void EnableBarriers()
    {
        Music.Instance.StartMusicFade(true);
        
        for (int i = 0; i < barriers.Length; i++)
            barriers[i].SetActive(true);
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

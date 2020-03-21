using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerController : MonoBehaviour
{
    public List<WaveSpawner> spawners;
    public float numberOfWaves;
    public bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<WaveSpawner>() != null)
            spawners[i] = transform.GetChild(i).GetComponent<WaveSpawner>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsPlayerInRange(bool _state)
    {
        playerInRange = _state;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDetectPlayer : MonoBehaviour
{
    public int layerIndex;
    public WaveSpawnerController waveController;

    void Start()
    {
        waveController = GetComponentInParent<WaveSpawnerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerIndex)
        {
            waveController.IsPlayerInRange(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == layerIndex)
        {
            waveController.IsPlayerInRange(false);
        }
    }
}

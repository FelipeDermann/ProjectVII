using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDetectPlayer : MonoBehaviour
{
    public int layerIndex;
    public WaveSpawnerController waveController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerIndex)
        {
            if(waveController != null) waveController.IsPlayerInRange();
            else
            {
                waveController = transform.parent.GetComponentInChildren<WaveSpawnerController>();
            }
        }
    }
}

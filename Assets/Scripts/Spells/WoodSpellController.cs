using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpellController : MonoBehaviour
{
    public WoodSpell firstWoodAttack;
    public float maxNumberOfWaves;
    public float currentWavesToSpawn;

    private void Start()
    {
        firstWoodAttack.woodSpellController = this;
        currentWavesToSpawn = maxNumberOfWaves;
    }

    public void DecreaseWaveNumber()
    {
        currentWavesToSpawn -= 1;
    }
}

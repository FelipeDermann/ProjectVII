using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalSpellParticleSpawner : MonoBehaviour
{
    public float particleSpawnTime, particleDestroyTime;
    public Vector3 offset;
    public ParticleSystem finalBurst;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnBurst", particleSpawnTime);
    }

    void SpawnBurst()
    {
        var burst = Instantiate(finalBurst.gameObject, transform.position + offset, Quaternion.identity);
        Destroy(burst, particleDestroyTime);
    }
}

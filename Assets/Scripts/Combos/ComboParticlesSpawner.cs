using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboParticlesSpawner : MonoBehaviour
{
    public GameObject comboFX;
    public float destroyParticleTimer;
    public Vector3 rotParticle, posParticle;

    // Start is called before the first frame update
    void Start()
    {
        var combo = Instantiate(comboFX, transform.position + posParticle, Quaternion.Euler(rotParticle));
        Destroy(combo, destroyParticleTimer);
    }
}
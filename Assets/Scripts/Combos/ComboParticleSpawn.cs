using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboParticleSpawn : MonoBehaviour
{
    public GameObject comboFX;
    public float destroyParticleTimer, rotX, rotY, rotZ, posX, posY, posZ;

    // Start is called before the first frame update
    void Start()
    {
        var combo = Instantiate(comboFX, transform.position + new Vector3(posX, posY, posZ), Quaternion.Euler(new Vector3(rotX, rotY, rotZ)));
        Destroy(combo, destroyParticleTimer);
    }
}

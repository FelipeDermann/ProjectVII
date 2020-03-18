using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHitbox : MonoBehaviour
{
    [Header("Main Attributes")]
    public int layerIndex;
    public int damageToDeal;
    public float knockbackPower;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerIndex)
        {
            Debug.Log("DAMAGED PLAYER");
            other.GetComponent<PlayerStatus>().DecreaseHealth(damageToDeal);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHitbox : MonoBehaviour
{
    [Header("Main Attributes")]
    public int layerIndex;
    public int damageToDeal;

    public float knockbackPower;
    public float knockbackTime;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerIndex)
        {
            GameObject player = other.gameObject;
            PlayerMovement playerMove = player.GetComponent<PlayerMovement>();

            Vector3 knockbackDirection = player.transform.position - transform.position;
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            playerMove.KnockBack(knockbackDirection, knockbackPower, knockbackTime);
            player.GetComponent<PlayerStatus>().TakeDamage(damageToDeal);
        }
    }
}

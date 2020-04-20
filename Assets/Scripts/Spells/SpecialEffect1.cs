using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    Transform playerPos;

    public float knockbackForce;
    public float knockupForce;
    public float knockTime;

    public float damage;

    void Start()
    {
        playerPos = FindObjectOfType<PlayerMovement>().transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            var enemyMove = other.GetComponent<EnemyMove>();

            Vector3 knockbackDirection = playerPos.position - other.transform.position;
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            enemy.TakeDamage(damage);
        }

    }
}

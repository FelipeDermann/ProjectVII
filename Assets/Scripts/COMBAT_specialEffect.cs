using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_specialEffect : MonoBehaviour
{
    Transform playerPos;

    public float knockbackForce;
    public float knockupForce;
    public float knockTime;

    public KnockType knockType;

    public float damage;

    void Start()
    {
        playerPos = FindObjectOfType<COMBAT_PlayerMovement>().transform;
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

            switch (knockType)
            {
                case KnockType.Back:
                    enemyMove.KnockBack(-knockbackDirection, knockbackForce, knockTime);
                    break;
                case KnockType.Away:
                    enemyMove.KnockAway(-knockbackDirection, knockbackForce, knockTime);
                    break;
                case KnockType.Up:
                    enemyMove.KnockUp(-knockbackDirection, knockbackForce, knockTime);
                    break;
            }

            enemy.TakeDamage(damage);
        }

    }
}

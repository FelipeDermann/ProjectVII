using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpike : MonoBehaviour
{
    Transform playerPos;
    EarthSpellManager manager;

    float damage;
    float knockbackForce;
    float knockupForce;
    float knockTime;
    KnockType knockType;

    private void Start()
    {
        manager = GetComponentInParent<EarthSpellManager>();

        playerPos = GetComponentInParent<EarthSpellManager>().playerPos;
        knockbackForce = manager.knockbackForce;
        knockupForce = manager.knockupForce;
        knockTime = manager.knockTime;
        knockType = manager.knockType;
        damage = manager.damage;
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

            enemy.DecreaseHealth(damage);

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

        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpell : MonoBehaviour
{
    [Header("Basic Attributes")]
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;

    public KnockType knockType;

    public float damage;
    public float projectileSpeed;
    public float projectileLifeTime;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * projectileSpeed;

        Destroy(gameObject, projectileLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            var enemyMove = other.gameObject.GetComponent<EnemyMove>();

            Vector3 knockbackDirection = transform.position - other.transform.position;
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

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

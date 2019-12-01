using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpecial : MonoBehaviour
{
    Rigidbody rb;
    Spell spell;

    [Header("Basic Attributes")]
    public float speed;
    public float damage;
    public float explosionRadius;
    public GameObject explosionparticle;
    public LayerMask enemyLayerMask;

    [Header("Knockback")]
    public float knockbackForce;
    public float knockTime;
    public KnockType knockType;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        spell = GetComponent<Spell>();
    }

    void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayerMask);

        if (colliders.Length > 0)
        {
            foreach (Collider currentEnemy in colliders)
            {
                var enemy = currentEnemy.gameObject.GetComponent<Enemy>();
                var enemyMove = currentEnemy.gameObject.GetComponent<EnemyMove>();

                Vector3 knockbackDirection = transform.position - currentEnemy.transform.position;
                knockbackDirection.Normalize();
                knockbackDirection.y = 0;

                enemy.TakeDamage(damage);

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            Collider col = GetComponent<Collider>();
            col.enabled = false;

            GameObject particle = Instantiate(explosionparticle, transform.position, transform.rotation);
            Destroy(particle, 2);

            Explosion();

            Destroy(gameObject);
        }
    }
}

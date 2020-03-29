using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSpecial : MonoBehaviour
{
    Rigidbody rb;
    Vector3 direction;
    MeshRenderer mesh;
    Transform playerPos;

    [Header("Basic Attributes")]
    public float speed;
    public float timeToStart;
    public float timeToDestroyAfterImpact;

    [Header("Blade Death Particle Effect")]
    public GameObject deathParticle;
    public float timeToDestroyParticle;
    public Vector3 deathParticleOffset;

    [Header("Knockback")]
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;

    public KnockType knockType;

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindObjectOfType<Attack>().transform;

        rb = GetComponent<Rigidbody>();
        mesh = GetComponentInChildren<MeshRenderer>();
        mesh.enabled = false;

        Invoke(nameof(Move), timeToStart);
    }

    void Move()
    {
        mesh.enabled = true;

        Vector3 dir = transform.right;
        direction = -dir.normalized;

        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            var enemyMove = other.gameObject.GetComponent<EnemyMove>();

            Vector3 knockbackDirection = playerPos.position - other.transform.position;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            CapsuleCollider capsule = GetComponent<CapsuleCollider>();
            capsule.enabled = false;

            BoxCollider box = GetComponentInChildren<BoxCollider>();
            box.enabled = false;

            Destroy(gameObject, timeToDestroyAfterImpact);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        var burst = Instantiate(deathParticle, transform.position + deathParticleOffset, Quaternion.identity);
        Destroy(burst, timeToDestroyParticle);
    }
}

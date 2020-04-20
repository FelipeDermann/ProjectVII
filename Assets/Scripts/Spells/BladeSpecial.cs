using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSpecial : MonoBehaviour
{
    Rigidbody rb;
    Vector3 direction;
    MeshRenderer mesh;
    CapsuleCollider capsule;
    BoxCollider box;

    [Header("Assign the audio emitters here")]
    public AudioEmitter startEmitter;
    public AudioEmitter burstEmitter;

    [Header("Do not alter")]
    public Transform playerPos;
    public Vector3 initialPos;
    public Vector3 initialRot;

    [Header("Basic Attributes")]
    public float speed;
    public float timeToStart;
    public float lifetime;
    public float timeToDestroyAfterImpact;

    [Header("Blade Death and Particle Effect")]
    public ParticleSystem deathParticle;
    public ParticleSystem trailParticle;
    public float timeToDestroyParticle;
    public Vector3 deathParticleOffset;

    [Header("Knockback")]
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;

    public float damage;

    public void StartBlade(Transform _playerPos)
    {
        playerPos = _playerPos;

        rb = GetComponent<Rigidbody>();
        box = GetComponentInChildren<BoxCollider>();
        capsule = GetComponent<CapsuleCollider>();

        mesh = GetComponentInChildren<MeshRenderer>();
        mesh.enabled = true;

        initialPos = transform.localPosition;
        initialRot = transform.localEulerAngles;

        capsule.enabled = true;
        box.enabled = true;
        rb.isKinematic = false;

        trailParticle.Play();

        Invoke(nameof(Move), timeToStart);
        Invoke(nameof(CallEndBladeEarly), lifetime);
    }

    void Move()
    {
        startEmitter.PlaySound();

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

            if (playerPos == null) playerPos = GameObject.FindObjectOfType<PlayerMovement>().transform;

            Vector3 knockbackDirection = playerPos.position - other.transform.position;
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            enemyMove.KnockBack(knockbackDirection, knockbackForce, knockupForce, knockTime);
            enemy.TakeDamage(damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            capsule.enabled = false;
            box.enabled = false;

            Invoke(nameof(CallEndBlade), timeToDestroyAfterImpact);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            capsule.enabled = false;
            box.enabled = false;

            EndBlade();
        }
    }

    void CallEndBladeEarly()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        capsule.enabled = false;
        box.enabled = false;

        EndBlade();
    }

    void CallEndBlade()
    {
        EndBlade();
    }

    void EndBlade()
    {
        CancelInvoke();

        trailParticle.Stop();
        mesh.enabled = false;
        deathParticle.Play();

        burstEmitter.PlaySound();

        Invoke(nameof(CallReturnToOriginalPosition), 4);
    }

    void CallReturnToOriginalPosition()
    {
        ReturnToOriginalPosition();
    }

    void ReturnToOriginalPosition()
    {
        CancelInvoke();
        transform.localPosition = initialPos;
        transform.localEulerAngles = initialRot;
    }
}

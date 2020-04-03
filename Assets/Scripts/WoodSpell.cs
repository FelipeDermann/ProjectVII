using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpell : MonoBehaviour
{
    [Header("Knockback")]
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;

    public KnockType knockType;

    [Header("Particles")]
    public ParticleSystem parentParticle;
    public ParticleSystem glowParticle;
    public ParticleSystem root;


    [Header("Basic Attributes")]
    public float damage;
    public float projectileSpeed;
    public float projectileLifeTime;
    public float timeToRemoveObjectAfterDeath;

    [Header("Others")]
    public AudioEmitter audioEmitter;

    Rigidbody rb;
    PoolableObject thisObject;
    Collider col;

    public void StartSpell()
    {
        if (col == null) col = GetComponent<Collider>();
        if(rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * projectileSpeed;

        //root.startRotation3D = new Vector3(transform.rotation.x/Mathf.PI, transform.rotation.y / Mathf.PI, transform.rotation.z / Mathf.PI);
        parentParticle.Play();
        audioEmitter.PlaySound();
        col.enabled = true;

        Invoke(nameof(CallEndEffect), projectileLifeTime);
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
            EndEffect();
        }
    }

    void CallEndEffect()
    {
        EndEffect();
    }

    void EndEffect()
    {
        CancelInvoke();

        rb.velocity = Vector3.zero;
        //rb.velocity = -transform.up * projectileSpeed;
        //root.transform.parent = null;
        parentParticle.Stop();
        glowParticle.Clear();

        audioEmitter.FadeOut();
        col.enabled = false;

        if (thisObject == null) thisObject = GetComponent<PoolableObject>();
        GameManager.Instance.WoodSpellPool.ReturnObject(thisObject, timeToRemoveObjectAfterDeath);
    }
}

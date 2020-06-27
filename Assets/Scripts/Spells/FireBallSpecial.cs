using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpecial : MonoBehaviour
{
    Rigidbody rb;
    Spell spell;

    [Header("Basic Attributes")]
    public float speed;
    public float lifetime;
    public float timeToReturnObjectToPoolAfterDeath;
    public float damage;
    public float explosionRadius;

    [Header("Particles")]
    public ParticleSystem explosionParticle;
    public ParticleSystem fireBallParticle;
    public ParticleSystem fireBallTrailParticle;
    Vector3 enemyDir;
    Vector3 hitPoint;

    [Header("Others")]
    public LayerMask enemyLayerMask;
    public AudioEmitter audioEmitterFireBall;
    public AudioEmitter audioEmitterBurning;
    public AudioEmitter audioEmitterExplosion;
    Collider col;
    PoolableObject thisObject;

    [Header("Knockback")]
    public float knockbackForce;
    public float knockUpForce;
    public float knockTime;

    public void StartSpell()
    {
        if (thisObject == null) thisObject = GetComponent<PoolableObject>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (col == null) col = GetComponent<Collider>();
        
        rb.velocity = transform.forward * speed;

        fireBallTrailParticle.gameObject.SetActive(true);
        fireBallParticle.Play();
        audioEmitterFireBall.PlaySoundWithPitch();
        audioEmitterBurning.PlaySoundWithPitch();

        col.enabled = true;

        Invoke(nameof(CallDeath), lifetime);
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

                Vector3 knockbackDirection = currentEnemy.transform.position - transform.position;
                enemyDir = currentEnemy.transform.position - transform.position;
                enemyDir.Normalize();
                knockbackDirection.Normalize();
                knockbackDirection.y = 0;

                //rickmartin
                Ray ray = new Ray(transform.position, enemyDir);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask))
                {
                    hitPoint = hit.point;
                    var hitmarker = GameManager.Instance.hitMarkerPool.RequestObject(hit.point + new Vector3(0, 1, 0), transform.rotation);
                    var hitmarkerParticleSystem = hitmarker.GetComponent<ParticleSystem>();
                    hitmarkerParticleSystem.Play();

                    GameManager.Instance.hitMarkerPool.ReturnObject(hitmarker, 2);
                }
                else
                {
                    var hitmarker = GameManager.Instance.hitMarkerPool.RequestObject(enemy.transform.position + new Vector3(0, 1, 0), transform.rotation);
                    var hitmarkerParticleSystem = hitmarker.GetComponent<ParticleSystem>();
                    hitmarkerParticleSystem.Play();

                    GameManager.Instance.hitMarkerPool.ReturnObject(hitmarker, 2);
                }

                enemyMove.KnockBack(knockbackDirection, knockbackForce, knockUpForce, knockTime);
                enemy.TakeDamage(damage);

            }
        }
    }

    void CallDeath()
    {
        Death();
    }

    void Death()
    {
        CancelInvoke();
        col.enabled = false;

        fireBallTrailParticle.gameObject.SetActive(false);
        fireBallParticle.Stop();
        explosionParticle.Play();

        audioEmitterExplosion.PlaySoundWithPitch();
        audioEmitterBurning.FadeOut();
        rb.velocity = Vector3.zero;

        Explosion();

        GameManager.Instance.FireSpellPool.ReturnObject(thisObject, timeToReturnObjectToPoolAfterDeath);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Death();
        }
    }

   
}

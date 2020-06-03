using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpell : MonoBehaviour
{
    [Header("Knockback")]
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;

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
    public List<Collider> enemyColliders;

    Rigidbody rb;
    PoolableObject thisObject;
    Collider col;

    public LayerMask enemyLayerMask;
    Vector3 enemyDir;
    Vector3 hitPoint;

    public void StartSpell()
    {
        if (col == null) col = GetComponent<Collider>();
        if(rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * projectileSpeed;

        //root.startRotation3D = new Vector3(transform.rotation.x/Mathf.PI, transform.rotation.y / Mathf.PI, transform.rotation.z / Mathf.PI);
        parentParticle.Play();
        audioEmitter.PlaySoundWithPitch();
        col.enabled = true;

        Invoke(nameof(CallEndEffect), projectileLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            var enemyMove = other.gameObject.GetComponent<EnemyMove>();
            if (enemyColliders.Contains(other.GetComponent<Collider>())) return;

            Vector3 knockbackDirection = transform.position - other.transform.position;
            enemyDir = other.transform.position - transform.position;
            enemyDir.Normalize();
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            //rickmartin
            Ray ray = new Ray(transform.position, enemyDir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask))
            {
                print(hit.point + " on object: " + hit.transform.name);
                hitPoint = hit.point;
                var hitmarker = GameManager.Instance.hitMarkerPool.RequestObject(hit.point, transform.rotation);
                var hitmarkerParticleSystem = hitmarker.GetComponent<ParticleSystem>();
                hitmarkerParticleSystem.Play();

                GameManager.Instance.hitMarkerPool.ReturnObject(hitmarker, 2);
            }

            enemyColliders.Add(other.GetComponent<Collider>());

            enemyMove.KnockBack(knockbackDirection, knockbackForce, knockupForce, knockTime);
            enemy.TakeDamage(damage);
        }

        if (other.CompareTag("Wall") || other.CompareTag("Fence"))
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
        enemyColliders.Clear();

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

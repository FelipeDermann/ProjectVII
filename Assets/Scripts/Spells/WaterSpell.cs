﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpell : MonoBehaviour
{
    [Header("Basic Attributes")]
    public float timeUntilNextHit;
    public float maxNumberOfHits;
    public float damagePerHit;

    float numberOfHitsDone;
    public float timeToRemoveObjectAfterFinalHit;

    bool canApplyDamage;
    public ParticleSystem tornadoParticle;

    Vector3 enemyDir;
    Vector3 hitPoint;

    [Header("Knockback")]
    public float KnockbackForce;
    public float KnockbackTimeToRecover;
    public float smallKnockupForce;
    public float knockbackForceOnFinalHit;
    public float hurtTime;

    [Header("Others")]
    public AudioEmitter audioEmitter;
    Spell spell;
    BoxCollider boxCollider;
    public LayerMask enemyLayerMask;
    PoolableObject thisObject;

    [Header("Spawn Check")]
    public LayerMask layerMask;
    public BoxCollider spawnCheckHitbox;

    public void CheckIfCanSpawn()
    {
        Vector3 boxBounds = spawnCheckHitbox.bounds.extents;
        Collider[] walls = Physics.OverlapBox(transform.position, boxBounds, transform.rotation, layerMask);
        bool mustDestroy = false;

        foreach (Collider currentWall in walls)
        {
            if (currentWall.gameObject.CompareTag("Wall"))
            {
                Debug.Log("HIT WALL");
                mustDestroy = true;
            }
        }

        if (mustDestroy) ReturnObjectToPool();
        else
        {
            StartSpell();
        }
    }

    public void StartSpell()
    {
        numberOfHitsDone = 0;

        boxCollider = GetComponent<BoxCollider>();
        spell = GetComponent<Spell>();

        tornadoParticle.Play();
        audioEmitter.PlaySoundWithPitch();

        Invoke(nameof(ApplyDamage), .5f);
    }

    void ApplyDamage()
    {
        Vector3 boxBounds = boxCollider.bounds.extents;
        Collider[] colliders = Physics.OverlapBox(boxCollider.transform.position, boxBounds, boxCollider.transform.rotation, enemyLayerMask);

        if (colliders.Length > 0)
        {
            foreach (Collider currentEnemy in colliders)
            {
                var enemy = currentEnemy.gameObject.GetComponent<Enemy>();
                var enemyMove = currentEnemy.gameObject.GetComponent<EnemyMove>();

                Vector3 knockbackDirection = currentEnemy.transform.position - spell.playerPos.position;
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

                //if (numberOfHitsDone != maxNumberOfHits) enemyMove.SlightKnockUp(-knockbackDirection, KnockbackForce, smallKnockupForce, KnockbackTimeToRecover);
                //else enemyMove.KnockUp(-knockbackDirection, knockbackForceOnFinalHit, hurtTime);

                enemyMove.KnockBack(knockbackDirection, KnockbackForce, smallKnockupForce, hurtTime);
                enemy.TakeDamage(damagePerHit);

            }
        }
        
        numberOfHitsDone += 1;
        StopApplyingDamage();
    }

    void StopApplyingDamage()
    {
        if (numberOfHitsDone >= maxNumberOfHits)
        {
            PrepareToDestroy();
            return;
        }

        Invoke(nameof(ApplyDamage), timeUntilNextHit);
    }

    void PrepareToDestroy()
    {
        audioEmitter.FadeOut();

        tornadoParticle.Stop();
    }

    void ReturnObjectToPool()
    {
        if (thisObject == null) thisObject = GetComponent<PoolableObject>();
        GameManager.Instance.WaterSpellPool.ReturnObject(thisObject, timeToRemoveObjectAfterFinalHit);
    }
}

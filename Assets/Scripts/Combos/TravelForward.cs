﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TravelForward : MonoBehaviour
{
    public static event Action GenerationCycleHit;
    public static event Action EnemyHit;

    Collider col;
    Rigidbody rb;
    public PlayerStatus player;

    [Header("Basic Attributes")]
    public float speed;
    public float timeToStop;
    float damage;

    float knockbackForce;
    float knockupForce;
    float knockbackTime;
    public bool controlCycleEffect;
    public bool generationCycleEffect;
    public bool canApplyGenerationCycle;

    public List<Collider> enemyColliders;

    public void ActivateControlCycleEffect()
    {
        controlCycleEffect = true;
    }
    public void ActivateGenerationCycleEffect()
    {
        generationCycleEffect = true;
        canApplyGenerationCycle = true;
    }
    public void GainSpeed(PlayerStatus _playerStatus)
    {
        player = _playerStatus;

        if (col == null) col = GetComponent<Collider>();
        if(rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        Invoke(nameof(Stop), timeToStop);
    }

    void Stop()
    {
        controlCycleEffect = false;
        generationCycleEffect = false;
        canApplyGenerationCycle = false;

        rb.velocity = Vector3.zero;
        if (col != null) col.enabled = false;
        transform.localPosition = Vector3.zero;

        foreach(Collider collider in enemyColliders)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collider, false);
        }
        enemyColliders.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            var enemyMove = other.GetComponent<EnemyMove>();

            Vector3 knockbackDirection = other.transform.position - transform.position;
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            ComboEffect comboEffect = GetComponentInParent<ComboEffect>();
            damage = comboEffect.damage;
            knockbackForce = comboEffect.knockbackForce;
            knockbackTime = comboEffect.knockbackTime;
            knockupForce = comboEffect.knockupForce;

            enemyMove.KnockBack(knockbackDirection, knockbackForce, knockupForce, knockbackTime);
            enemy.TakeDamage(damage);
            if (controlCycleEffect) other.GetComponent<EnemyDebuff>().GainStack();
            if (generationCycleEffect && canApplyGenerationCycle)
            {
                canApplyGenerationCycle = false;
                GenerationCycleHit?.Invoke();
                Debug.Log(other.name + " WAS HIT WITH GENERATION CYCLE ON WATER COMBO");
            }

            EnemyHit?.Invoke();

            if (!enemy.dead && !enemyMove.knockedDown) player.IncreaseMana();

            enemyColliders.Add(other.GetComponent<Collider>());
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>());
        }

    }
}

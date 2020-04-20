using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    Animator anim;
    TargetScript targetScriptForLockOn;
    public float currentHealth;
    public float maxHealth;

    public int numberOfCoinsToDrop;

    public float timeUntilDeath;
    public int layerWhenDead;

    public bool dead;
    public bool invincible;

    public GameObject deathParticles;
    public Transform deathParticlesSpawnPoint;

    public EnemySpawner mySpawner;
    public WaveSpawner myWaveSpawner;

    //public LifeBar lifeBar;
    public LifeBarEnemy lifeBar;
    EnemyMove move;

    public static event Action EnemyDead;

    void Start()
    {
        anim = GetComponent<Animator>();
        lifeBar = GetComponentInChildren<LifeBarEnemy>();
        move = GetComponent<EnemyMove>();
        targetScriptForLockOn = GetComponentInChildren<TargetScript>();

        currentHealth = maxHealth;
    }

    public void GetHurt()
    {
        anim.SetTrigger("hit");
    }

    public void CallDeathRoutine()
    {
        StartCoroutine(nameof(EnemyDeath));
    }

    public void Invincibility(float _time)
    {
        StartCoroutine(nameof(InvincibilityCoroutine), _time);
    }

    IEnumerator InvincibilityCoroutine(float _time)
    {
        invincible = true;
        yield return new WaitForSeconds(_time);
        invincible = false;
    }

    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(timeUntilDeath);

        GameObject deathParticleClone = Instantiate(deathParticles, deathParticlesSpawnPoint.position, transform.rotation);

        if (mySpawner != null) mySpawner.CallSpawnEnemy();
        if (myWaveSpawner != null) myWaveSpawner.SpawnedEnemyDead();

        Destroy(deathParticleClone, 4);
        Destroy(gameObject);
    }

    void EnemyDeathSimple()
    {
        GameObject deathParticleClone = Instantiate(deathParticles, deathParticlesSpawnPoint.position, transform.rotation);

        if (mySpawner != null) mySpawner.CallSpawnEnemy();
        if (myWaveSpawner != null) myWaveSpawner.SpawnedEnemyDead();

        Destroy(deathParticleClone, 4);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (dead) return;
        if (move.knockedDown) return;
        if (invincible) return;

        currentHealth -= damage;
        CheckIfDead();

        lifeBar.UpdateLifeBar(currentHealth, maxHealth);
    }

    public void DecreaseHealth(float damage)
    {
        if (dead) return;
        if (move.knockedDown) return;

        currentHealth -= damage;

        lifeBar.UpdateLifeBar(currentHealth, maxHealth);
    }

    public void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            lifeBar.DisableBar();

            targetScriptForLockOn.RemoveFromList();

            gameObject.layer = layerWhenDead;

            anim.SetBool("dead", true);
            dead = true;

            SpawnCoins();

            EnemyDeathSimple();
        }
    }

    void SpawnCoins()
    {
        for (int i = 0; i < numberOfCoinsToDrop; i++)
        {
            PoolableObject coin = GameManager.Instance.CoinPool.RequestObject(transform.position + Vector3.up/2, transform.rotation);
            coin.GetComponent<Coin>().Activate();
        }
    }

    void OnDestroy()
    {
        EnemyDead?.Invoke();
    }

}

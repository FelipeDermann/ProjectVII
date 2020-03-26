using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    public float currentHealth;
    public float maxHealth;

    public float timeUntilDeath;

    public bool dead;

    public GameObject deathParticles;
    public Transform deathParticlesSpawnPoint;

    public EnemySpawner mySpawner;
    public WaveSpawner myWaveSpawner;

    //public LifeBar lifeBar;
    public LifeBarEnemy lifeBar;
    EnemyMove move;

    void Start()
    {
        anim = GetComponent<Animator>();
        lifeBar = GetComponentInChildren<LifeBarEnemy>();
        move = GetComponent<EnemyMove>();

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

    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(timeUntilDeath);

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

            anim.SetTrigger("die");
            anim.SetBool("dead", true);
            dead = true;
        }
    }

}

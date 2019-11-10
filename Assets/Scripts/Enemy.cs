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

    void Start()
    {
        anim = GetComponent<Animator>();

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

        Destroy(deathParticleClone, 4);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (dead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            anim.SetTrigger("die");
            anim.SetBool("dead", true);
            dead = true;
        }
    }

}

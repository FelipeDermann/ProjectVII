using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public enum EnemyType
{
    Normal,
    Claws
}

public class Enemy : MonoBehaviour
{
    public bool beingTargetedByLockOn;
    [SerializeField]
    private EnemyDebuff enemyDebuff;
    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;
    [SerializeField]
    private SkinnedMeshRenderer meshRenderer2;
    public ObjectPool poolToReturnTo;
    [SerializeField]
    private PoolableObject thisObject;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private CapsuleCollider capsuleCol1;
    [SerializeField]
    private CapsuleCollider capsuleCol2;
    [SerializeField]
    private TargetScript targetScriptForLockOn;
    [SerializeField]
    private NavMeshAgent agent;

    [Header("Health")]
    public float currentHealth;
    public float maxHealth;

    public int numberOfCoinsToDrop;

    [Header("Death")]
    public float timeUntilReturnToPool;
    public int layerWhenDead;

    public bool dead;
    public bool invincible;
    public bool isElite;

    public GameObject deathParticles;
    public Transform deathParticlesSpawnPoint;

    [Header("Others")]
    public LifeBarEnemy lifeBar;
    [SerializeField]
    private EnemyMove move;
    [SerializeField]
    private EnemyAttack attack;

    [Header("Spawners - do not alter")]
    public EnemySpawner mySpawner;
    public WaveSpawner myWaveSpawner;

    [Header("Particles")]
    public ParticleSystem hitParticle;
    public ParticleSystem eliteParticle;

    public static event Action EnemyDead;
    public static event Action RemoveLockOnTarget;

    void OnEnable()
    {
        WeaponHitbox.AttackEnded += CanBeHurtAgainBySword;
    }

    void OnDisable()
    {
        WeaponHitbox.AttackEnded -= CanBeHurtAgainBySword;
    }

    void CanBeHurtAgainBySword()
    {
        invincible = false;
    }

    public void Activate()
    {
        if (isElite) eliteParticle.Play();

        currentHealth = maxHealth;

        capsuleCol1.enabled = true;
        capsuleCol2.enabled = true;
        agent.enabled = true;
        rb.isKinematic = true;
        meshRenderer.enabled = true;
        meshRenderer2.enabled = true;
        dead = false;

        move.Activate();
        attack.Activate();
        lifeBar.EnableBar();
        lifeBar.UpdateLifeBar(currentHealth, maxHealth);

        currentHealth = maxHealth;
    }

    void Deactivate()
    {
        if (isElite) eliteParticle.Stop();
     
        capsuleCol1.enabled = false;
        capsuleCol2.enabled = false;
        agent.enabled = false;
        meshRenderer.enabled = false;
        meshRenderer2.enabled = false;
        rb.isKinematic = true;

        enemyDebuff.Deactivate();

        //GameManager.Instance.EnemyPool.ReturnObject(thisObject, timeUntilReturnToPool);
        EnemyDead?.Invoke();
        poolToReturnTo.ReturnObject(thisObject, timeUntilReturnToPool);
    }

    public void GetHurt()
    {
        anim.SetTrigger("hit");
    }

    public void HitParticle()
    {
        hitParticle.Play();
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
        yield return new WaitForSeconds(timeUntilReturnToPool);

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

        if(beingTargetedByLockOn)
        {
            beingTargetedByLockOn = false;
            RemoveLockOnTarget?.Invoke();
        }
        Deactivate();
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

            //gameObject.layer = layerWhenDead;

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

}

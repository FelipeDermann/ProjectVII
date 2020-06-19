using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class ComboEffect : MonoBehaviour
{
    public static event Action GenerationCycleHit;
    public static event Action EnemyHit;

    [Header("--References (don't alter them)--")]
    public PlayerStatus playerStatus;
    public Transform playerPos;
    public Collider hitbox;
    public PoolableObject thisObject;
    public AudioEmitter audioEmitter;
    public ElementType element;

    [Header("--Place Combo Effect Here--")]
    public ParticleSystem comboFX;

    [Header("Knockback")]
    public float knockbackForce;
    public float knockbackTime;
    public float knockupForce;
    public float invincibilityTime;

    [Header("Basic Attributes")]
    public float damage;
    public float hitboxLifetime;
    public float particleLifetime;
    public float audioLifetime;
    public float timeToRemoveEntireObject;

    public bool controlCycleEffect;
    public bool generationCycleEffect;
    public bool canApplyGenerationCycle;

    Vector3 enemyDir;
    Vector3 hitPoint;
    public LayerMask enemyLayerMask;
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

    void Awake()
    {
        if (comboFX == null) comboFX = transform.GetChild(0).GetComponent<ParticleSystem>();
        if (hitbox == null) hitbox = GetComponent<Collider>();
        if (thisObject == null) thisObject = GetComponent<PoolableObject>();
        if (audioEmitter == null) audioEmitter = GetComponentInChildren<AudioEmitter>();

        if (hitbox != null) hitbox.enabled = true;
        comboFX.Play();
        audioEmitter.PlaySoundWithPitch();

        Invoke(nameof(DisableHitbox), hitboxLifetime);
        Invoke(nameof(DisableParticle), particleLifetime);
        Invoke(nameof(DisableAudio), audioLifetime);
        ReturnObjectToPool(timeToRemoveEntireObject);
    }

    void OnDestroy()
    {
        //comboFX.Stop();
    }

    void DisableHitbox()
    {
        if(hitbox != null) hitbox.enabled = false;
    }

    void DisableParticle()
    {
        comboFX.Stop();
    }

    void DisableAudio()
    {
        audioEmitter.FadeOut();
    }

    void ReturnObjectToPool(float _time)
    {
        generationCycleEffect = false;
        canApplyGenerationCycle = false;
        controlCycleEffect = false;
        enemyColliders.Clear();

        switch (element)
        {
            case ElementType.Fire:
                GameManager.Instance.FireComboPool.ReturnObject(thisObject, _time);
                break;
            case ElementType.Water:
                GameManager.Instance.WaterComboPool.ReturnObject(thisObject, _time);
                break;
            case ElementType.Metal:
                GameManager.Instance.MetalComboPool.ReturnObject(thisObject, _time);
                break;
            case ElementType.Wood:
                GameManager.Instance.WoodComboPool.ReturnObject(thisObject, _time);
                break;
            case ElementType.Earth:
                GameManager.Instance.EarthComboPool.ReturnObject(thisObject, _time);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            var enemyMove = other.GetComponent<EnemyMove>();
            if (enemyColliders.Contains(other.GetComponent<Collider>())) return;

            Vector3 knockbackDirection = other.transform.position - playerPos.position;
            enemyDir = other.transform.position - transform.position;
            enemyDir.Normalize();
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            enemyMove.KnockBack(knockbackDirection, knockbackForce, knockupForce, knockbackTime);
            enemy.TakeDamage(damage);
            if (controlCycleEffect) other.GetComponent<EnemyDebuff>().GainStack();
            if (generationCycleEffect && canApplyGenerationCycle)
            {
                canApplyGenerationCycle = false;
                GenerationCycleHit?.Invoke();
                Debug.Log(other.name + " WAS HIT WITH GENERATION CYCLE ON GENERAL COMBO");
            }

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

            EnemyHit?.Invoke();

            if (!enemy.dead) playerStatus.IncreaseMana();
            enemyColliders.Add(other.GetComponent<Collider>());
        }

    }
}

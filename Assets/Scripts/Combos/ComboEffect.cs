using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KnockType
{
    Back,
    Away,
    Up
}

public class ComboEffect : MonoBehaviour
{
    [Header("--References (don't alter them)--")]
    public PlayerStatus playerStatus;
    public Transform playerPos;
    public Collider hitbox;
    public PoolableObject thisObject;
    public AudioEmitter audioEmitter;
    public Element element;

    [Header("--Place Combo Effect Here--")]
    public ParticleSystem comboFX;

    [Header("Knockback")]
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;
    public KnockType knockType;

    [Header("Basic Attributes")]
    public float damage;
    public float hitboxLifetime;
    public float particleLifetime;
    public float audioLifetime;
    public float timeToRemoveEntireObject;


    void OnEnable()
    {
        if (comboFX == null) comboFX = transform.GetChild(0).GetComponent<ParticleSystem>();
        if (hitbox == null) hitbox = GetComponent<Collider>();
        if (thisObject == null) thisObject = GetComponent<PoolableObject>();
        if (audioEmitter == null) audioEmitter = GetComponentInChildren<AudioEmitter>();

        if (hitbox != null) hitbox.enabled = true;
        comboFX.Play();
        audioEmitter.PlaySound();

        Invoke(nameof(DisableHitbox), hitboxLifetime);
        Invoke(nameof(DisableParticle), particleLifetime);
        Invoke(nameof(DisableAudio), audioLifetime);
        ReturnObjectToPool(timeToRemoveEntireObject);
    }

    void OnDisable()
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
        switch (element)
        {
            case Element.Fire:
                GameManager.Instance.FireComboPool.ReturnObject(thisObject, _time);
                break;
            case Element.Water:
                GameManager.Instance.WaterComboPool.ReturnObject(thisObject, _time);
                break;
            case Element.Metal:
                GameManager.Instance.MetalComboPool.ReturnObject(thisObject, _time);
                break;
            case Element.Wood:
                GameManager.Instance.WoodComboPool.ReturnObject(thisObject, _time);
                break;
            case Element.Earth:
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

            Vector3 knockbackDirection = playerPos.position - other.transform.position;
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            enemy.TakeDamage(damage);

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

            if(!enemy.dead && !enemyMove.knockedDown) playerStatus.IncreaseMana();
        }

    }
}

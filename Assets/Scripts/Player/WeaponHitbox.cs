﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public bool active;
    public PlayerStatus playerStatus;
    public PlayerSounds playerSounds;

    Transform playerPos;

    public MeleeWeaponTrail weaponTrail;

    public float knockbackForce;
    public float knockTime;

    float lightDamage;
    float heavyDamage;

    public ParticleSystem[] attackTrail;
    public ParticleSystem[] attackParticle;
    public float particlePosXOn, particlePosXOff, particleLenghtOn, particleLenghtOff;


    private void OnEnable()
    {
        DashBehaviour.DashStart += DeactivateHitbox;
        DisableAttackState.FinishedAttack += DeactivateHitbox;

        PlayerAnimation.EndHitbox += DeactivateHitbox;
        PlayerAnimation.StartHitbox += ActivateHitbox;

        PlayerAnimation.EndTrail += TrailOff;
        PlayerAnimation.StartTrail += TrailOn;
    }
    private void OnDisable()
    {
        DashBehaviour.DashStart += DeactivateHitbox;
        DisableAttackState.FinishedAttack -= DeactivateHitbox;

        PlayerAnimation.EndHitbox -= DeactivateHitbox;
        PlayerAnimation.StartHitbox -= ActivateHitbox;

        PlayerAnimation.EndTrail += TrailOff;
        PlayerAnimation.StartTrail += TrailOn;
    }

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        playerStatus = GetComponentInParent<PlayerStatus>();
        playerSounds = GetComponentInParent<PlayerSounds>();
        playerPos = FindObjectOfType<PlayerMovement>().transform;

        lightDamage = playerStatus.lightAttackDamage;
        heavyDamage = playerStatus.heavyAttackDamage;

        weaponTrail = GetComponentInParent<MeleeWeaponTrail>();
        weaponTrail.Emit = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) TrailOn();
        if (Input.GetKeyDown(KeyCode.X)) TrailOff();
    }

    void TrailOn()
    {
        //weaponTrail.Emit = true;
        foreach (var particle in attackParticle)
        {
            var particleShape = particle.shape;
            particleShape.radius = particleLenghtOn;
            particleShape.position = new Vector3(particlePosXOn, 0, 0);
        }
        foreach (var trail in attackTrail)
        {
            trail.Play();
        }
    }
    void TrailOff()
    {
        //weaponTrail.Emit = false;
        foreach (var particle in attackParticle)
        {
            var particleShape = particle.shape;
            particleShape.radius = particleLenghtOff;
            particleShape.position = new Vector3(particlePosXOff, 0, 0);
        }
        foreach (var trail in attackTrail)
        {
            trail.Stop();
        }
    }

    public void ActivateHitbox()
    {
        active = true;
    }

    public void DeactivateHitbox()
    {
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<Enemy>();
                var enemyMove = other.GetComponent<EnemyMove>();

                Vector3 knockbackDirection = playerPos.position - other.transform.position;
                knockbackDirection.Normalize();
                knockbackDirection.y = 0;

                if (!enemy.dead && !enemyMove.knockedDown) playerSounds.PlaySlashSound();

                enemyMove.KnockBack(-knockbackDirection, knockbackForce, knockTime);
                enemy.TakeDamage(lightDamage);

                if (!enemy.dead && !enemyMove.knockedDown) playerStatus.IncreaseMana();


            }
        }
    }
}

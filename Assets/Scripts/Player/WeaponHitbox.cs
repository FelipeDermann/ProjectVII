﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public bool active;
    public PlayerStatus playerStatus;

    Transform playerPos;

    public MeleeWeaponTrail weaponTrail;

    public float knockbackForce;
    public float knockTime;

    public float damage;

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
        playerPos = FindObjectOfType<PlayerMovement>().transform;

        weaponTrail = GetComponentInParent<MeleeWeaponTrail>();
        weaponTrail.Emit = false;
    }

    void TrailOn()
    {
        weaponTrail.Emit = true;
    }
    void TrailOff()
    {
        weaponTrail.Emit = false;
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

                enemyMove.KnockBack(-knockbackDirection, knockbackForce, knockTime);
                enemy.TakeDamage(damage);

                if (!enemy.dead && !enemyMove.knockedDown) playerStatus.IncreaseMana();

            }
        }
    }
}
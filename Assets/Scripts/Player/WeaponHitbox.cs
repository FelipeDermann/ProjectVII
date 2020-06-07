using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponHitbox : MonoBehaviour
{
    public static event Action EnemyHit;
    public static event Action AttackEnded;

    [Header("Hitmarker")]
    public Transform swordHiltPos;
    public Transform swordTipPos;
    public LayerMask enemyLayerMask;
    Vector3 enemyDir;
    Vector3 hitPoint;

    public bool active;
    public PlayerStatus playerStatus;
    public PlayerMovement playerMove;

    Transform playerPos;

    public MeleeWeaponTrail weaponTrail;

    public float knockbackForce;
    public float knockTime;
    public AttackType attackType;

    float lightDamage;
    float heavyDamage;

    public ParticleSystem[] attackTrail;
    public ParticleSystem[] attackParticle;
    public float particlePosXOn, particlePosXOff, particleLenghtOn, particleLenghtOff;

    public List<Collider> enemyColliders;

    private void OnEnable()
    {
        PlayerAnimation.LightAttackDamage += SetAttackType;
        PlayerAnimation.HeavyAttackDamage += SetAttackType;

        DashBehaviour.DashStart += DeactivateHitbox;
        DisableAttackState.FinishedAttack += DeactivateHitbox;

        PlayerAnimation.EndHitbox += DeactivateHitbox;
        PlayerAnimation.StartHitbox += ActivateHitbox;

        PlayerAnimation.EndTrail += TrailOff;
        PlayerAnimation.StartTrail += TrailOn;
        AttackAnimationBehaviour.StopTrail += TrailOff;
    }

    private void OnDisable()
    {
        PlayerAnimation.LightAttackDamage -= SetAttackType;
        PlayerAnimation.HeavyAttackDamage -= SetAttackType;

        DashBehaviour.DashStart += DeactivateHitbox;
        DisableAttackState.FinishedAttack -= DeactivateHitbox;

        PlayerAnimation.EndHitbox -= DeactivateHitbox;
        PlayerAnimation.StartHitbox -= ActivateHitbox;

        PlayerAnimation.EndTrail += TrailOff;
        PlayerAnimation.StartTrail += TrailOn;
        AttackAnimationBehaviour.StopTrail -= TrailOff;
    }

    public void SetAttackType(AttackType _attackType)
    {
        attackType = _attackType;
    }

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        playerStatus = GetComponentInParent<PlayerStatus>();
        playerPos = FindObjectOfType<PlayerMovement>().transform;
        playerMove = GetComponentInParent<PlayerMovement>();

        weaponTrail = GetComponentInParent<MeleeWeaponTrail>();
        weaponTrail.Emit = false;
    }

    void TrailOn()
    {
        weaponTrail.Emit = true;
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
        weaponTrail.Emit = false;
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
        if (playerMove.dashing) return;
        active = true;
    }

    public void DeactivateHitbox()
    {
        AttackEnded?.Invoke();
        active = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<Enemy>();
                var enemyMove = other.GetComponent<EnemyMove>();
                if (enemy.invincible) return;

                Vector3 knockbackDirection = other.transform.position - playerPos.position;
                enemyDir = other.transform.position - transform.position;
                enemyDir.Normalize();
                knockbackDirection.Normalize();
                knockbackDirection.y = 0;

                //rickmartin
                Ray ray = new Ray(transform.position, enemyDir);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask))
                {
                    hitPoint = hit.point;
                    var hitmarker = GameManager.Instance.hitMarkerPool.RequestObject(hit.point, transform.rotation);
                    var hitmarkerParticleSystem = hitmarker.GetComponent<ParticleSystem>();
                    hitmarkerParticleSystem.Play();

                    GameManager.Instance.hitMarkerPool.ReturnObject(hitmarker, 2);
                }

                //hit sound
                if (!enemy.dead && !enemyMove.knockedDown)
                {
                    var audioEmitter = GameManager.Instance.AudioSlashPool.RequestObject(other.transform.position, transform.rotation);
                    var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

                    emitterScript.PlaySoundWithPitch();
                    float clipLength = emitterScript.clipToPlay.length;
                    GameManager.Instance.AudioSlashPool.ReturnObject(audioEmitter, clipLength + 1);
                }

                if (!enemy.isElite) enemyMove.KnockBack(knockbackDirection, knockbackForce, 0, knockTime);

                if (attackType == AttackType.LIGHT)
                {
                    lightDamage = playerStatus.lightAttackDamage;
                    enemy.TakeDamage(lightDamage);
                }
                if (attackType == AttackType.HEAVY)
                {
                    heavyDamage = playerStatus.heavyAttackDamage;
                    enemy.TakeDamage(heavyDamage);
                }

                EnemyHit?.Invoke();

                if (!enemy.dead && !enemyMove.knockedDown) playerStatus.IncreaseMana();
                enemy.invincible = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = new Ray(transform.position, enemyDir);
        Gizmos.DrawRay(ray);

        Gizmos.DrawSphere(hitPoint, .2f);
    }
}

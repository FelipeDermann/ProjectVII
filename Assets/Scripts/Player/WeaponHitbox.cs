using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public bool active;
    public Magic playerMagic;

    Transform playerPos;

    public MeleeWeaponTrail weaponTrail;

    public float knockbackForce;
    public float knockTime;

    public float damage;

    private void OnEnable()
    {
        DisableAttackState.FinishedAttack += DeactivateHitbox;
        AttackAnimationBehaviour.AttackFinished += DeactivateHitbox;
        AttackAnimationBehaviour.StartHitbox += EnableCoroutine;
    }
    private void OnDisable()
    {
        DisableAttackState.FinishedAttack -= DeactivateHitbox;
        AttackAnimationBehaviour.AttackFinished -= DeactivateHitbox;
        AttackAnimationBehaviour.StartHitbox -= EnableCoroutine;
    }

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        playerMagic = GetComponentInParent<Magic>();
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

    void EnableCoroutine(float _time)
    {
        StartCoroutine(nameof(ActivateHitboxCountdown), _time);
    }

    IEnumerator ActivateHitboxCountdown(float _time)
    {
        yield return new WaitForSeconds(_time);
        ActivateHitbox();
    }

    public void ActivateHitbox()
    {
        active = true;
        TrailOn();
    }

    public void DeactivateHitbox()
    {
        active = false;
        TrailOff();
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

                if (!enemy.dead && !enemyMove.knockedDown) playerMagic.GainEnergy();

            }
        }
    }
}

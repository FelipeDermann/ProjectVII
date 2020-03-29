using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpell : MonoBehaviour
{
    [Header("Basic Attributes")]
    public float timeUntilNextHit;
    public float timeHitboxIsActive;
    public float maxNumberOfHits;
    public float damagePerHit;

    public float numberOfHitsDone;
    public float timeUntilDeath;

    bool canApplyDamage;
    public ParticleSystem tornadoParticle;

    [Header("Knockback")]
    public float KnockbackForce;
    public float KnockbackTimeToRecover;
    public float smallKnockupForce;
    public float knockbackForceOnFinalHit;
    public float hurtTime;

    Spell spell;
    BoxCollider boxCollider;
    public LayerMask enemyLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        spell = GetComponent<Spell>();

        Invoke(nameof(ApplyDamage), .5f);
    }

    void ApplyDamage()
    {
        Vector3 boxBounds = boxCollider.bounds.extents;
        Collider[] colliders = Physics.OverlapBox(boxCollider.transform.position, boxBounds, boxCollider.transform.rotation, enemyLayerMask);

        if (colliders.Length > 0)
        {
            foreach (Collider currentEnemy in colliders)
            {
                var enemy = currentEnemy.gameObject.GetComponent<Enemy>();
                var enemyMove = currentEnemy.gameObject.GetComponent<EnemyMove>();

                Vector3 knockbackDirection = spell.playerPos.position - currentEnemy.transform.position;
                knockbackDirection.Normalize();
                knockbackDirection.y = 0;

                if (numberOfHitsDone != maxNumberOfHits) enemyMove.SlightKnockUp(-knockbackDirection, KnockbackForce, smallKnockupForce, KnockbackTimeToRecover);
                else enemyMove.KnockUp(-knockbackDirection, knockbackForceOnFinalHit, hurtTime);

                enemy.DecreaseHealth(damagePerHit);

            }
        }
        
        numberOfHitsDone += 1;
        StopApplyingDamage();
    }

    void StopApplyingDamage()
    {
        if (numberOfHitsDone >= maxNumberOfHits)
        {
            PrepareToDestroy();
            return;
        }

        Invoke(nameof(ApplyDamage), timeUntilNextHit);
    }

    void PrepareToDestroy()
    {
        tornadoParticle.Stop();
        Destroy(gameObject, timeUntilDeath);
    }
}

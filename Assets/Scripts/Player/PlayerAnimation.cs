using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimation : MonoBehaviour
{
    public static Action StartHitbox;
    public static event Action EndHitbox;

    public static event Action HurtAnimation;

    public static event Action<bool> DashSpeedStart;
    public static event Action<bool> DashSpeedEnd;

    public static event Action<AttackType> LightAttackDamage;
    public static event Action<AttackType> HeavyAttackDamage;

    public static event Action<bool> AttackMoveStart;
    public static event Action<bool> AttackMoveEnd;

    public static event Action StartTrail;
    public static event Action EndTrail;

    public static event Action SpawnMagicHitbox;
    public static event Action SpawnComboHitbox;

    public static event Action StartSpellAnimation;
    public static event Action TurnToEnemyIfLockedOn;

    public static event Action StartNextAttackInput;

    public static event Action RestartScene;

    public float secondsOfInputDetection;

    public void Hurt()
    {
        HurtAnimation?.Invoke();
    }

    public void LightAttack()
    {
        LightAttackDamage?.Invoke(AttackType.LIGHT);
    }

    public void HeavyAttack()
    {
        HeavyAttackDamage?.Invoke(AttackType.HEAVY);
    }

    public void AttackMovementStart()
    {
        AttackMoveStart?.Invoke(true);
    }
    public void AttackMovementEnd()
    {
        AttackMoveEnd?.Invoke(false);
    }

    public void DashStart()
    {
        DashSpeedStart?.Invoke(true);
    }
    public void DashEnd()
    {
        DashSpeedEnd?.Invoke(false);
    }

    public void CastingAnimation()
    {
        StartSpellAnimation?.Invoke();
    }
    public void TurnToEnemy()
    {
        TurnToEnemyIfLockedOn?.Invoke();
    }

    public void RestartSceneAfterTime()
    {
        RestartScene?.Invoke();
    }

    public void EnableInput()
    {
        StartNextAttackInput?.Invoke();
    }

    public void ActivateTrail()
    {
        StartTrail?.Invoke();
    }

    public void DeactivateTrail()
    {
        EndTrail?.Invoke();
    }

    public void ActivateHitbox()
    {
        StartHitbox?.Invoke();
        //Debug.Log("HITBOX ON");
    }

    public void DeactivateHitbox()
    {
        EndHitbox?.Invoke();
        //Debug.Log("HITBOX OFF");
    }

    public void StartCombo()
    {
        SpawnComboHitbox?.Invoke();
    }

    public void StartSpell()
    {
        SpawnMagicHitbox?.Invoke();
    }
}

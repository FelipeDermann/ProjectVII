using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_Animator : MonoBehaviour
{
    COMBAT_MovementInput inputs;
    COMBAT_PlayerMovement move;
    COMBAT_Attack attack;
    COMBAT_Magic playerMagic;

    Animator anim;

    COMBAT_WeaponHitbox weapon;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponentInParent<COMBAT_PlayerMovement>();
        inputs = GetComponentInParent<COMBAT_MovementInput>();
        attack = GetComponentInParent<COMBAT_Attack>();
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<COMBAT_WeaponHitbox>();
        playerMagic = GetComponentInParent<COMBAT_Magic>();
    }

    public void DashEnd()
    {
        move.DashEnd();
    }

    public void TurnToEnemy()
    {
        attack.LookToClosestEnemy();
    }

    public void MagicAttackCall()
    {
        playerMagic.MagicAttack();
    }

    public void WeaponHitboxActivate()
    {
        weapon.ActivateHitbox();
    }

    public void WeaponHitboxDeactivate()
    {
        weapon.DeactivateHitbox();
    }

    public void DisableMove()
    {
        inputs.canMove = false;
    }

    public void EnableMove()
    {
        Debug.Log("Can move again");
        inputs.canMove = true;
        attack.canInputNextAttack = false;
    }

    public void EnableNextAttack()
    {
        attack.canInputNextAttack = true;
    }

    public void DisablePreviousInput()
    {
        attack.canInputNextAttack = false;
    }
}

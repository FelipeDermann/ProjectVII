using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    EnemyMove enemyMove;

    [Header("Colliders")]
    public Collider attackArea;
    public Collider attackHitbox;
    
    [Header("Wait Times and Attributes")]
    public float timeToWaitBeforeAttacking;
    public float timeBeforeAttackingAgain;
    public bool playerInRange;
    public bool attacking;
    public bool canAttack;
    public bool hurt;

    public void Activate()
    {
        hurt = false;
        canAttack = true;
    }

    public void ChangeHurtState(bool _state)
    {
        hurt = _state;
        animator.SetBool("hurt", _state);
    }

    public void PlayerInRange(bool _playerInRange)
    {
        playerInRange = _playerInRange;

        PrepareToAttack();
    }

    void PrepareToAttack()
    {
        if (!playerInRange || !canAttack || hurt) return;

        CancelInvoke();
        Invoke(nameof(Attack), timeToWaitBeforeAttacking);
        enemyMove.ChangeCanMoveState(false);
        //animator.SetFloat("Blend", 0);
    }

    void Attack()
    {
        attacking = true;
        animator.SetBool("attacking", true);
        animator.SetTrigger("attack");

        enemyMove.TurnToPlayer();
    }

    public void AttackEnd()
    {
        attacking = false;
        canAttack = false;

        CancelInvoke();
        Invoke(nameof(CanAttackAgain), timeBeforeAttackingAgain);

        animator.SetBool("attacking", false);

        enemyMove.ChangeCanMoveState(true);
    }

    void CanAttackAgain()
    {
        canAttack = true;

        PrepareToAttack();
    }
}

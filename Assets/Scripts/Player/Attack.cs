using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Combo
{
    None,
    WaterCombo,
    FireCombo,
    EarthCombo,
    MetalCombo,
    WoodCombo
}

public class Attack : MonoBehaviour
{
    public Animator anim;
    MovementInput move;
    PlayerMovement playerMove;
    Elements elements;
    LockOn lockOn;

    public int inputsHeavy;
    public int inputsLight;

    public bool canInputNextAttack;
    public bool attacking;

    public BoxCollider getEnemyArea;
    public LayerMask enemyLayerMask;

    public List<string> inputs = new List<string>();

    [Header("Combos (Choose which element corresponds to which combo)")]
    public Combo L_L_H;
    public Combo L_H_H;
    public Combo L_H_L;
    public Combo H_L_H;
    public Combo H_H_L;
    public Combo H_L_L;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponentInParent<MovementInput>();
        playerMove = GetComponentInParent<PlayerMovement>();
        elements = GetComponent<Elements>();
        lockOn = GetComponent<LockOn>();
    }

    private void OnEnable()
    {
        AttackAnimationBehaviour.StartedAttack += StartAttacking;
        AttackAnimationBehaviour.StartedAttack += LookToEnemy;
        DashBehaviour.DashStart += StopAttacking;

        PlayerAnimation.StartNextAttackInput += EnableNextAttackInput;
        DisableAttackState.FinishedAttack += StopAttacking;
    }
    private void OnDisable()
    {
        AttackAnimationBehaviour.StartedAttack -= StartAttacking;
        AttackAnimationBehaviour.StartedAttack -= LookToEnemy;
        DashBehaviour.DashStart -= StopAttacking;

        PlayerAnimation.StartNextAttackInput -= EnableNextAttackInput;
        DisableAttackState.FinishedAttack -= StopAttacking;
    }

    private void StartAttacking()
    {
        attacking = true;
        anim.SetBool("attacking", true);
    }
    private void StopAttacking()
    {
        anim.SetBool("attacking", false);

        DisableNextAttackInput();
        attacking = false;
        ClearInputList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) LookToEnemy();

        LightAttack();
        HeavyAttack();

        if (Input.GetKeyDown(KeyCode.F)) CheckInputCombination();
    }

    public void LookToEnemy()
    {
        //Look to closest enemy if not locked on
        if (!lockOn.isLocked)
        {
            Vector3 boxBounds = getEnemyArea.bounds.extents;
            Collider[] colliders = Physics.OverlapBox(getEnemyArea.transform.position, boxBounds, getEnemyArea.transform.rotation, enemyLayerMask);

            float distanceToClosestEnemy = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (Collider currentEnemy in colliders)
            {
                float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
                if (distanceToEnemy < distanceToClosestEnemy)
                {
                    distanceToClosestEnemy = distanceToEnemy;
                    closestEnemy = currentEnemy.transform;
                }
            }

            if (closestEnemy != null)
            {
                Debug.Log(closestEnemy.gameObject);
                transform.LookAt(new Vector3(closestEnemy.position.x, transform.position.y, closestEnemy.position.z));
            }
            else return;
        }
        //attack locked on enemy 
        else
        {
            lockOn.TurnToLockedEnemy();
        }
    }

    void LightAttack()
    {
        if (!Input.GetButtonDown("light")) return;
        if (anim.GetBool("casting")) return;
        if (playerMove.dashing) return;

        if (!attacking)
        {
            elements.currentCombo = Combo.None;
            inputs.Add("light");
            anim.SetTrigger("startL");
            ResetAnimTriggers();
        }
        if (attacking && canInputNextAttack)
        {
            canInputNextAttack = false;
            inputs.Add("light");

            if (inputs.Count >= 3)
            {
                if (CheckInputCombination()) return;
                else anim.SetTrigger("light");
            }
            else anim.SetTrigger("light");
        }
    }

    void HeavyAttack()
    {
        if (!Input.GetButtonDown("heavy")) return;
        if (anim.GetBool("casting")) return;
        if (playerMove.dashing) return;

        if (!attacking)
        {
            elements.currentCombo = Combo.None;
            inputs.Add("heavy");
            anim.SetTrigger("startH");
            ResetAnimTriggers();
        }
        if (attacking && canInputNextAttack)
        {
            canInputNextAttack = false;
            inputs.Add("heavy");

            if (inputs.Count >= 3)
            {
                if (CheckInputCombination()) return;
                else anim.SetTrigger("heavy");
            }
            else anim.SetTrigger("heavy");
        }
    }

    void ResetAnimTriggers()
    {
        anim.ResetTrigger("light");
        anim.ResetTrigger("heavy");
        anim.ResetTrigger("WaterCombo");
        anim.ResetTrigger("MetalCombo");
        anim.ResetTrigger("EarthCombo");
        anim.ResetTrigger("FireCombo");
        anim.ResetTrigger("WoodCombo");
    }

    public void EnableNextAttackInput()
    {
        canInputNextAttack = true;
    }
    public void DisableNextAttackInput()
    {
        canInputNextAttack = false;
    }

    public bool CheckInputCombination()
    {
        bool comboIsTriggered = false;
        Combo triggerToActivate = Combo.None;

        if (inputs[0] == "light" && inputs[1] == "light" && inputs[2] == "heavy") triggerToActivate = L_L_H;
        if (inputs[0] == "light" && inputs[1] == "heavy" && inputs[2] == "heavy") triggerToActivate = L_H_H;
        if (inputs[0] == "light" && inputs[1] == "heavy" && inputs[2] == "light") triggerToActivate = L_H_L;
        if (inputs[0] == "heavy" && inputs[1] == "heavy" && inputs[2] == "light") triggerToActivate = H_H_L;
        if (inputs[0] == "heavy" && inputs[1] == "light" && inputs[2] == "heavy") triggerToActivate = H_L_H;

        if (triggerToActivate != Combo.None)
        {
            comboIsTriggered = true;

            elements.currentCombo = triggerToActivate;
            anim.SetTrigger(triggerToActivate.ToString());
        }

        ClearInputList();

        return comboIsTriggered;
    }

    public void ClearInputList()
    {
        inputs.Clear();
    }
}

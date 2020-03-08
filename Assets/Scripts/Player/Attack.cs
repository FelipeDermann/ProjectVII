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
    Elements elements;

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
        elements = GetComponent<Elements>();
    }

    private void OnEnable()
    {
        AttackAnimationBehaviour.StartedAttack += StartAttacking;
        AttackAnimationBehaviour.StartedAttack += LookToClosestEnemy;

        NextInputBehaviour.StartNextAttackInput += EnableNextInputCoroutine;
        AttackAnimationBehaviour.StopNextAttackInput += DisableNextAttackInput;

        DisableAttackState.FinishedAttack += StopAttacking;
        DisableAttackState.FinishedAttack += DisableNextAttackInput;
    }
    private void OnDisable()
    {
        AttackAnimationBehaviour.StartedAttack -= StartAttacking;
        AttackAnimationBehaviour.StartedAttack -= LookToClosestEnemy;

        NextInputBehaviour.StartNextAttackInput -= EnableNextInputCoroutine;
        AttackAnimationBehaviour.StopNextAttackInput -= DisableNextAttackInput;

        DisableAttackState.FinishedAttack -= StopAttacking;
        DisableAttackState.FinishedAttack -= DisableNextAttackInput;
    }

    private void StartAttacking()
    {
        attacking = true;
    }
    private void StopAttacking()
    {
        attacking = false;
        ClearInputList();
    }

    void EnableNextInputCoroutine(float _time)
    {
        StartCoroutine(nameof(NextInputEnableCountdown), _time);
    }

    IEnumerator NextInputEnableCountdown(float _time)
    {
        yield return new WaitForSeconds(_time);
        EnableNextAttackInput();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) LookToClosestEnemy();

        LightAttack();
        HeavyAttack();

        if (Input.GetKeyDown(KeyCode.F)) CheckInputCombination();
    }

    public void LookToClosestEnemy()
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

    void LightAttack()
    {
        if (!Input.GetButtonDown("light")) return;

        if (!attacking)
        {
            elements.AllowComboHitboxSpawn();
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

        if (!attacking)
        {
            elements.AllowComboHitboxSpawn();
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

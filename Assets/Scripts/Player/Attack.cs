using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AttackType
{
    LIGHT,
    HEAVY
}

public class Attack : MonoBehaviour
{
    public Animator anim;
    MovementInput move;
    PlayerMovement playerMove;
    PlayerElements elements;
    LockOn lockOn;

    public int inputsHeavy;
    public int inputsLight;

    public bool canInputNextAttack;
    public bool attacking;
    public bool canAttack;

    public BoxCollider getEnemyArea;
    public LayerMask enemyLayerMask;

    public List<string> inputs = new List<string>();

    public Transform currentEnemyTarget;

    [Header("Combos (Choose which element corresponds to which combo)")]
    public Element L_L_H;
    public Element L_H_H;
    public Element L_H_L;
    public Element H_L_H;
    public Element H_H_L;
    public Element H_L_L;

    [Header("Rotation when attacking")]
    public float rotationSpeed;
    public float timeToFullyRotate;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponentInParent<MovementInput>();
        playerMove = GetComponentInParent<PlayerMovement>();
        elements = GetComponent<PlayerElements>();
        lockOn = GetComponent<LockOn>();
        cam = Camera.main;

        canAttack = true;
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
        currentEnemyTarget = null;
        playerMove.CanMoveWhenAttacking(false);

        DisableNextAttackInput();
        attacking = false;
        ClearInputList();
    }

    // Update is called once per frame
    void Update()
    {
        LightAttack();
        HeavyAttack();
    }

    public void LookToEnemy()
    {
        //Look to closest enemy if not locked on
        if (!lockOn.isLocked)
        {
            //Change direction if pressing one
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                currentEnemyTarget = null;
                ChangeDirection();
                return;
            }

            if (currentEnemyTarget == null)
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
                    //Debug.Log(closestEnemy.gameObject);
                    //transform.LookAt(new Vector3(closestEnemy.position.x, transform.position.y, closestEnemy.position.z));
                    StartCoroutine(nameof(TurnToEnemySmooth), closestEnemy);
                    currentEnemyTarget = closestEnemy;
                }
                else return;
            }
            else StartCoroutine(nameof(TurnToEnemySmooth), currentEnemyTarget);
        }
        //attack locked on enemy 
        else
        {
            lockOn.TurnToLockedEnemy();
        }
    }

    IEnumerator TurnToEnemySmooth(Transform _enemy)
    {
        var direction = _enemy.position - transform.position;

        float dot = 0;
        while (dot < 0.96f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.25f);

            dot = Vector3.Dot(transform.forward, (_enemy.position - transform.position).normalized);

            yield return null;
        }

        transform.LookAt(new Vector3(_enemy.position.x, transform.position.y, _enemy.position.z));
    }

    void ChangeDirection()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * InputZ + right * InputX;

        StartCoroutine(nameof(TurnToDirection), desiredMoveDirection);
    }

    IEnumerator TurnToDirection(Vector3 direction)
    {
        float time = 0;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.25f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed);

            yield return null;
        }
    }

    void LightAttack()
    {
        if (!Input.GetButtonDown("light")) return;
        if (anim.GetBool("casting")) return;
        if (playerMove.dashing) return;
        if (!canAttack) return;

        if (!attacking)
        {
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
        if (!canAttack) return;

        if (!attacking)
        { 
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
        anim.ResetTrigger("combo");
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
        Element elementToGive = null;

        if (inputs[0] == "light" && inputs[1] == "light" && inputs[2] == "heavy") elementToGive = L_L_H;
        if (inputs[0] == "light" && inputs[1] == "heavy" && inputs[2] == "heavy") elementToGive = L_H_H;
        if (inputs[0] == "light" && inputs[1] == "heavy" && inputs[2] == "light") elementToGive = L_H_L;
        if (inputs[0] == "heavy" && inputs[1] == "heavy" && inputs[2] == "light") elementToGive = H_H_L;
        if (inputs[0] == "heavy" && inputs[1] == "light" && inputs[2] == "heavy") elementToGive = H_L_H;

        if (elementToGive != null)
        {
            comboIsTriggered = true;

            elements.ChangeElements(elementToGive);
            anim.SetTrigger("combo");
        }

        ClearInputList();

        return comboIsTriggered;
    }

    public void ClearInputList()
    {
        inputs.Clear();
    }

    public void CanPlayerAttack(bool _state)
    {
        canAttack = _state;
    }
}

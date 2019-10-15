using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_Attack : MonoBehaviour
{
    public Animator anim;
    COMBAT_MovementInput move;

    public int inputsHeavy;
    public int inputsLight;

    public bool canCombo;

    public BoxCollider getEnemyArea;
    public LayerMask enemyLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponentInParent<COMBAT_MovementInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) LookToClosestEnemy();

        LightAttack();
        HeavyAttack();

        //give inputs to animator variables
        anim.SetInteger("heavy_inputs", inputsHeavy);
        anim.SetInteger("light_inputs", inputsLight);
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

        if (!canCombo)
        {
            ResetInputs();
            anim.ResetTrigger("light");
            anim.SetTrigger("startL");
        }
        else
        {
            Debug.Log("light");
            anim.SetTrigger("light");
        }
    }

    void HeavyAttack()
    {
        if (!Input.GetButtonDown("heavy")) return;

        if (!canCombo)
        {
            ResetInputs();
            anim.ResetTrigger("heavy");
            anim.SetTrigger("startH");
        }
        else
        {
            Debug.Log("heavy");
            anim.SetTrigger("heavy");
        }

    }

    void ResetInputs()
    {
        move.canMove = false;
        canCombo = true;

        inputsHeavy = 0;
        inputsLight = 0;
    }

    public void AddHeavy()
    {
        inputsHeavy += 1;
    }
    public void AddLight()
    {
        inputsLight += 1;
    }

    public void ActivateCanCombo()
    {
        canCombo = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_Attack : MonoBehaviour
{
    public Animator anim;
    COMBAT_MovementInput move;

    public int inputs;

    public bool canCombo;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponentInParent<COMBAT_MovementInput>();
    }

    // Update is called once per frame
    void Update()
    {
        LightAttack();
        HeavyAttack();
    }

    void LightAttack()
    {
        if (!Input.GetButtonDown("light")) return;

        if (!canCombo)
        {
            move.canMove = false;
            canCombo = true;
            anim.SetTrigger("light");
        }
        else anim.SetTrigger("continue");
    }

    void HeavyAttack()
    {
        if (!Input.GetButtonDown("heavy")) return;

        if (!canCombo)
        {
            move.canMove = false;
            canCombo = true;
            anim.SetTrigger("heavy");
        }
        else anim.SetTrigger("continue");

    }


}

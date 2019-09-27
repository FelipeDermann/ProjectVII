using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_Animator : MonoBehaviour
{
    COMBAT_MovementInput inputs;
    COMBAT_PlayerMovement move;
    COMBAT_Attack attack;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponentInParent<COMBAT_PlayerMovement>();
        inputs = GetComponentInParent<COMBAT_MovementInput>();
        attack = GetComponentInParent<COMBAT_Attack>();
        anim = GetComponent<Animator>();
    }

    public void DisableMove()
    {
        inputs.canMove = false;
    }

    public void EnableMove()
    {
        inputs.canMove = true;
        attack.canCombo = false;
    }

    public void EnableNextAttack()
    {
        attack.canCombo = true;
    }

    public void DisablePreviousInput()
    {
        attack.canCombo = false;
    }

    public void MoveForward()
    {
        move.MoveForwardCall();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicBehaviour : StateMachineBehaviour
{
    public static event Action<bool> UsingMagic;
    public static event Action StartMagicAnim;
    //public static event Action CancelComboHitbox;

    //[Header("Event Times")]
    //[Range(0.0f, 1.0f)]
    //public float timeToEnableHitbox;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("casting", true);
        UsingMagic?.Invoke(true);
        StartMagicAnim?.Invoke();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("casting", false);
        UsingMagic?.Invoke(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComboBehaviour : StateMachineBehaviour
{
    public static event Action<float> SpawnComboHitbox;
    public static event Action<float> ChangeElement;

    [Header("Event Times")]
    [Range(0.0f, 1.0f)]
    public float timeToEnableHitbox;
    [Range(0.0f, 1.0f)]
    public float timeToChangeElement;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float timeHitbox = timeToEnableHitbox * stateInfo.length;
        float timeChangeElement = timeToChangeElement * stateInfo.length;

        SpawnComboHitbox?.Invoke(timeHitbox);
        ChangeElement?.Invoke(timeChangeElement);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

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
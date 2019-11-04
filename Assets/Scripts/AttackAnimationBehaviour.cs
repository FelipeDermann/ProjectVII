using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackAnimationBehaviour : StateMachineBehaviour
{
    public static event Action StartedAttack;
    public static event Action<float> StartHitbox;
    public static event Action<float> StartMoveForward;

    public static event Action AttackFinished;
    public static event Action StopNextAttackInput;

    [Header("Event Times")]
    [Range(0.0f, 1.0f)]
    public float timeToEnableHitbox;
    [Range(0.0f, 1.0f)]
    public float timeToMoveForward;

    [TextArea]
    public string Observação = "Esse número é a porcentagem da animação. " +
    "0 é 0% e 1 é o 100% de sua duração";

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float timeHitbox = timeToEnableHitbox * stateInfo.length;
        float timeMoveForward = timeToMoveForward * stateInfo.length;

        StartedAttack?.Invoke();

        StartHitbox?.Invoke(timeHitbox);
        StartMoveForward?.Invoke(timeMoveForward);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AttackFinished?.Invoke();
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

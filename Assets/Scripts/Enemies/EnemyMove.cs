using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Enemy enemy;
    [SerializeField]
    EnemyGroundDetect groundDetect;

    public bool canMove;
    public bool knocked;
    public bool knockedDown;
    public bool playerInRange;
    public bool airborne;

    [Header("Target point that the enemy is running towards")]
    public Transform target;
    
    [Header("Time that enemy will chase player before giving up")]
    public float chaseTime;

    GameObject playerPointToLookAt;

    public void Activate()
    {
        canMove = true;
        knocked = false;
    }

    private void Update()
    {
        if (canMove && !knocked)
        {
            if (target != null) 
                if(target.position != agent.destination && agent.isActiveAndEnabled) agent.SetDestination(target.position);
        }
        if (!knocked) anim.SetFloat("Blend", agent.velocity.magnitude);
    }

    public void KnockBack(Vector3 _direction, float forceAmount, float _knockUpForce, float time)
    {
        if (enemy.dead) return;
        CancelInvoke(nameof(MoveAgain));
        knocked = true;

        //agent.isStopped = true;
        agent.enabled = false;
        rb.isKinematic = false;

        rb.velocity = Vector3.zero;

        Vector3 forceToApply = _direction * forceAmount;
        forceToApply.y = _knockUpForce;

        if (_knockUpForce > 0)
        {
            anim.SetBool("airborne", true);
            airborne = true;
            groundDetect.timeToMoveAgain = time;
        }

        rb.AddForce( forceToApply, ForceMode.Impulse);

        anim.SetTrigger("hit");

        if (!airborne)
        {
            if (IsInvoking(nameof(MoveAgain))) CancelInvoke(nameof(MoveAgain));
            Invoke(nameof(MoveAgain), time);
        }
        //enemy.CheckIfDead();
    }

    public void MoveAgain()
    {
        if (enemy.dead) return;
        Debug.Log("MOVING AGAIN FAM");
        airborne = false;
        anim.SetBool("airborne", false);

        agent.enabled = true;
        rb.isKinematic = true;
        knocked = false;
    }

    public void TurnToPlayer()
    {
        if(playerPointToLookAt == null) playerPointToLookAt = GameObject.FindGameObjectWithTag("PlayerHead");
        transform.LookAt(new Vector3(playerPointToLookAt.transform.position.x, transform.position.y, playerPointToLookAt.transform.position.z));
    }

    public void ChangeCanMoveState(bool _state)
    {
        canMove = _state;
    }

    public void SetTarget(Transform _target)
    {
        CancelInvoke(nameof(RemoveTarget));
        target = _target;
    }

    public void IsPlayerInRange(bool _state)
    {
        playerInRange = _state;

        if (!_state) Invoke(nameof(RemoveTarget), chaseTime);
    }

    void RemoveTarget()
    {
        target = null;
        if(agent.isActiveAndEnabled) agent.SetDestination(transform.position);
    }

    public void WarnPlayerOfDeath()
    {
        if (target != null) target.GetComponent<LockOn>().CameraLockOff();
    }

    //IEnumerator MoveForward()
    //{
    //    float wait = attackMoveTime;

    //    Vector3 movement = Vector3.zero;
    //    movement = transform.forward * attackMoveSpeed;

    //    while (wait > 0)
    //    {
    //        wait -= Time.deltaTime;
    //        controller.Move(movement * Time.deltaTime);

    //        yield return null;

    //    }
    //}
}

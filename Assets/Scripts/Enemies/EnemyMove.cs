using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Rigidbody rb;
    Enemy enemy;

    public bool canMove;
    public bool knocked;
    public bool knockedDown;
    public bool playerInRange;

    [Header("Target point that the enemy is running towards")]
    public Transform target;
    
    [Header("Time that enemy will chase player before giving up")]
    public float chaseTime;

    GameObject playerPointToLookAt;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //target = FindObjectOfType<PlayerMovement>().transform;
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha3)) TurnToPlayer();
        if (canMove && !knocked)
        {
            if (target != null) 
                if(target.position != agent.destination) agent.SetDestination(target.position);
        }
        if (!knocked) anim.SetFloat("Blend", agent.velocity.magnitude);
    }

    public void KnockBack(Vector3 _direction, float forceAmount, float time)
    {
        if (enemy.dead) return;
        if (knockedDown) return;

        knocked = true;

        agent.enabled = false;
        rb.isKinematic = false;

        rb.velocity = Vector3.zero;
        rb.AddForce(_direction * forceAmount, ForceMode.Impulse);

        anim.SetTrigger("hit");

        if (IsInvoking(nameof(MoveAgain))) CancelInvoke(nameof(MoveAgain));
        Invoke(nameof(MoveAgain), time);

        enemy.CheckIfDead();
    }

    public void SlightKnockUp(Vector3 _direction, float forceAmount, float upForceAmount, float time)
    {
        if (enemy.dead) return;
        if (knockedDown) return;

        //knockedDown = true;

        agent.enabled = false;
        rb.isKinematic = false;

        rb.velocity = Vector3.zero;
        rb.AddForce(_direction * forceAmount + new Vector3(0, upForceAmount, 0), ForceMode.Impulse);
        knocked = true;

        anim.SetTrigger("hit");

        if (IsInvoking(nameof(MoveAgain))) CancelInvoke(nameof(MoveAgain));
        Invoke(nameof(MoveAgain), time);

        enemy.CheckIfDead();
    }

    public void KnockAway(Vector3 _direction, float forceAmount, float time)
    {
        if (enemy.dead) return;
        if (knockedDown) return;

        TurnToPlayer();

        knockedDown = true;
        anim.SetBool("knockedDown", true);

        agent.enabled = false;
        rb.isKinematic = false;

        rb.velocity = Vector3.zero;
        rb.AddForce(_direction * forceAmount, ForceMode.Impulse);
        knocked = true;

        anim.SetTrigger("knockaway");

        enemy.CheckIfDead();
    }

    public void KnockUp(Vector3 _direction, float forceAmount, float time)
    {
        if (enemy.dead) return;
        if (knockedDown) return;

        TurnToPlayer();

        knockedDown = true;
        anim.SetBool("knockedDown", true);

        agent.enabled = false;
        rb.isKinematic = false;

        rb.velocity = Vector3.zero;
        rb.AddForce(_direction * forceAmount + new Vector3(0,5,0), ForceMode.Impulse);
        knocked = true;

        anim.SetTrigger("knockup");

        enemy.CheckIfDead();
    }

    public void MoveAgain()
    {
        if (enemy.dead) return;
        if (knockedDown) return;

        agent.enabled = true;
        rb.isKinematic = true;
        knocked = false;
    }

    public void MoveAfterKnockDown()
    {
        CancelInvoke(nameof(MoveAgain));

        knockedDown = false;
        anim.SetBool("knockedDown", false);

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
        agent.SetDestination(transform.position);
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

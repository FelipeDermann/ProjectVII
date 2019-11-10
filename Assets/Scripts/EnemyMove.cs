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
    
    public float speed;

    public Transform target;

    public float knockbackPower;
    public float knockedTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = FindObjectOfType<COMBAT_PlayerMovement>().transform;
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (canMove && !knocked)
        {
            if (target.position != agent.destination) agent.SetDestination(target.position);

            anim.SetFloat("Blend", agent.velocity.magnitude);
        }
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
    }

    public void KnockAway(Vector3 _direction, float forceAmount, float time)
    {
        if (enemy.dead) return;
        if (knockedDown) return;

        knockedDown = true;

        agent.enabled = false;
        rb.isKinematic = false;

        rb.velocity = Vector3.zero;
        rb.AddForce(_direction * forceAmount, ForceMode.Impulse);
        knocked = true;

        anim.SetTrigger("knockaway");
    }

    public void KnockUp(Vector3 _direction, float forceAmount, float time)
    {
        if (enemy.dead) return;
        if (knockedDown) return;

        knockedDown = true;

        agent.enabled = false;
        rb.isKinematic = false;

        rb.velocity = Vector3.zero;
        rb.AddForce(_direction * forceAmount + new Vector3(0,5,0), ForceMode.Impulse);
        knocked = true;

        anim.SetTrigger("knockup");
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

        agent.enabled = true;
        rb.isKinematic = true;
        knocked = false;
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

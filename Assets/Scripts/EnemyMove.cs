using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Rigidbody rb;

    public bool canMove;
    public bool knocked;
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

    public void KnockBack(Vector3 _direction)
    {
        agent.enabled = false;
        rb.isKinematic = false;

        rb.AddForce(_direction * knockbackPower, ForceMode.Impulse);
        knocked = true;

        if (IsInvoking(nameof(MoveAgain))) CancelInvoke(nameof(MoveAgain));
        Invoke(nameof(MoveAgain), knockedTime);
    }

    void MoveAgain()
    {
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

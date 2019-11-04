﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    COMBAT_Attack attack;
    private COMBAT_MovementInput input;
    
    public Animator anim;
    public float jumpForce;
    public bool jumped;
    public float attackMoveSpeed;
    public float attackMoveTime;

    [Header("Ground Detection")]
    public bool isGrounded;
    public bool canJump;
    public Transform boxPosition;
    public Vector3 boxSize;
    public LayerMask groundMask;

    [Header("Gravity")]
    public float gravity;
    public float maxFallSpeed;
    public float verticalSpeed;

    private void OnEnable()
    {
        DisableAttackState.FinishedAttack += CanWalkOn;
        AttackAnimationBehaviour.StartedAttack += CanWalkOff;
        AttackAnimationBehaviour.StartMoveForward += EnableMoveCoroutine;

        DashBehaviour.DashStart += DashStart;
        DashBehaviour.DashEnd += DashEnd;
    }
    private void OnDisable()
    {
        DisableAttackState.FinishedAttack -= CanWalkOn;
        AttackAnimationBehaviour.StartedAttack -= CanWalkOff;
        AttackAnimationBehaviour.StartMoveForward -= EnableMoveCoroutine;

        DashBehaviour.DashStart -= DashStart;
        DashBehaviour.DashEnd -= DashEnd;
    }
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<COMBAT_MovementInput>();
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        attack = GetComponent<COMBAT_Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Blend", input.Speed);

        canJump = !attack.canInputNextAttack;

        Dash();
        Jump();
        Gravity();
        DetectGround();
    }

    void Dash()
    {
        if (!Input.GetButtonDown("dash")) return;

        Debug.Log("DASH");
        anim.SetTrigger("dash");
    }

    public void DashStart()
    {
        canJump = false;
        CanWalkOn();
        //input.velocity = 16;
        attack.DisableNextAttackInput();
    }
    public void DashEnd()
    {
        canJump = true;
        input.velocity = 9;
    }

    public void CanWalkOn()
    {
        input.canMove = true;
        canJump = true;
    }

    public void CanWalkOff()
    {
        input.canMove = false;
        canJump = false;
    }

    void EnableMoveCoroutine(float _time)
    {
        StartCoroutine(nameof(MoveCountdownCoroutine), _time);
    }

    IEnumerator MoveCountdownCoroutine(float _time)
    {
        yield return new WaitForSeconds(_time);
        StartCoroutine(nameof(MoveForward));
    }

    IEnumerator MoveForward()
    {
        float wait = attackMoveTime;

        Vector3 movement = Vector3.zero;
        movement = transform.forward * attackMoveSpeed;

        while (wait > 0)
        {
            wait -= Time.deltaTime;
            controller.Move(movement * Time.deltaTime);

            yield return null;
        }
    }

    void Jump()
    {
        if (!isGrounded || !canJump) return;

        if(Input.GetButtonDown("Jump"))
        {
            //controller.SimpleMove
            verticalSpeed = jumpForce;
            jumped = true;
        }
    }

    void Gravity()
    {
        verticalSpeed -= gravity * Time.deltaTime;
        if (isGrounded && !jumped) verticalSpeed = -gravity * Time.deltaTime;
        if (!isGrounded) verticalSpeed -= gravity * Time.deltaTime;
        
        if (jumped) jumped = false;  

        Vector3 moveVector = Vector3.zero;
        moveVector.y = verticalSpeed;

        controller.Move(moveVector * Time.deltaTime);
    }

    void DetectGround()
    {
        if (Physics.CheckBox(boxPosition.position, boxSize, Quaternion.identity, groundMask))
        {
            isGrounded = true;
        }
        else isGrounded = false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(boxPosition.position, boxSize);
    }
}

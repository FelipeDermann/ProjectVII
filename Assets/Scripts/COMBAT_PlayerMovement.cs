using System.Collections;
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

        canJump = !attack.canCombo;

        Jump();
        Gravity();
        DetectGround();
    }

    public void MoveForwardCall()
    {
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

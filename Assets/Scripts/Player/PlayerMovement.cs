using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Attack attack;
    private MovementInput input;

    public Animator anim;
    public float jumpForce;
    public bool jumped;
    public bool canDash;
    public bool dashing;
    public float dashMoveSpeed;
    public float attackMoveSpeed;
    public float attackMoveTime;

    Camera cam;

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

        DashBehaviour.DashStart += DashStart;
        DashBehaviour.DashEnd += DashEnd;
    }
    private void OnDisable()
    {
        DisableAttackState.FinishedAttack -= CanWalkOn;
        AttackAnimationBehaviour.StartedAttack -= CanWalkOff;

        DashBehaviour.DashStart -= DashStart;
        DashBehaviour.DashEnd -= DashEnd;
    }
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<MovementInput>();
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        attack = GetComponent<Attack>();

        cam = Camera.main;

        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Blend", input.Speed);

        //canJump = !attack.canInputNextAttack;

        Dash();
        DashMove();
        //Jump();
        Gravity();
        DetectGround();
    }

    void Dash()
    {
        if (!Input.GetButtonDown("dash")) return;
        if (dashing) return;
        if (anim.GetBool("casting")) return;
        if (!canDash) return;

        //face input direction
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * InputZ + right * InputX;

        if (InputX != 0 || InputZ !=0) transform.rotation = Quaternion.LookRotation(desiredMoveDirection);

        //set variables do start the dash
        dashing = true;
        anim.SetTrigger("dash");
        anim.SetBool("dashing", dashing);
        Debug.Log("DASH");
    }

    public void DashStart()
    {
        canJump = false;
        CanWalkOff();

        attack.DisableNextAttackInput();
    }
    public void DashEnd()
    {
        Debug.Log("DASH END ");

        anim.ResetTrigger("dash");
        CanWalkOn();

        dashing = false;
        canJump = true;
        input.velocity = 9;
        anim.SetBool("dashing", dashing);
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
        //float wait = attackMoveTime;

        //Vector3 movement = Vector3.zero;
        //movement = transform.forward * attackMoveSpeed;

        //while (wait > 0)
        //{
        //    wait -= Time.deltaTime;
        //    controller.Move(movement * Time.deltaTime);

        //    yield return null;
        //}
        yield return null;
    }

    void DashMove()
    {
        if (dashing)
        {
            float wait = attackMoveTime;

            Vector3 movement = Vector3.zero;
            movement = transform.forward * dashMoveSpeed;

            Debug.Log(movement);

            controller.Move(movement * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (!isGrounded || !canJump) return;

        if (Input.GetButtonDown("Jump"))
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

    public void ChangeDashState(bool _state)
    {
        canDash = _state;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(boxPosition.position, boxSize);
    }
}
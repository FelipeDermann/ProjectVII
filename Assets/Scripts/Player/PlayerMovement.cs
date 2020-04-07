using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerStatus playerStatus;
    LockOn playerLockOn;
    CharacterController controller;
    Attack attack;
    private MovementInput input;

    public Animator anim;
    public float jumpForce;
    public bool jumped;
    public bool canDash;
    public bool dashing;
    public bool dashCanGainSpeed;
    public bool attackCanGainSpeed;
    public float dashMoveSpeed;
    public float attackMoveSpeed;
    Vector3 desiredMoveDirection;

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

        PlayerAnimation.DashSpeedStart += CanDashApplySpeed;
        PlayerAnimation.DashSpeedEnd += CanDashApplySpeed;

        PlayerAnimation.AttackMoveStart += CanMoveWhenAttacking;
        PlayerAnimation.AttackMoveEnd += CanMoveWhenAttacking;
    }
    private void OnDisable()
    {
        DisableAttackState.FinishedAttack -= CanWalkOn;
        AttackAnimationBehaviour.StartedAttack -= CanWalkOff;

        DashBehaviour.DashStart -= DashStart;
        DashBehaviour.DashEnd -= DashEnd;

        PlayerAnimation.DashSpeedStart -= CanDashApplySpeed;
        PlayerAnimation.DashSpeedEnd -= CanDashApplySpeed;

        PlayerAnimation.AttackMoveStart -= CanMoveWhenAttacking;
        PlayerAnimation.AttackMoveEnd -= CanMoveWhenAttacking;
    }
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<MovementInput>();
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        playerStatus = GetComponent<PlayerStatus>();
        playerLockOn = GetComponent<LockOn>();
        attack = GetComponent<Attack>();

        cam = Camera.main;

        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Blend", input.Speed);

        Dash();
        DashMove();
        AttackMove();
        //Jump();
        Gravity();
        //DetectGround();
    }

    void Dash()
    {
        if (!Input.GetButtonDown("dash")) return;
        if (anim.GetBool("casting")) return;
        if (dashing) return;
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

        dashCanGainSpeed = false;
        dashing = false;
        canJump = true;
        input.velocity = 9;
        anim.SetBool("dashing", dashing);
    }

    void DashMove()
    {
        if (dashCanGainSpeed)
        {
            //float wait = attackMoveTime;

            Vector3 movement = Vector3.zero;
            movement = transform.forward * dashMoveSpeed;

            //Debug.Log(movement);

            controller.Move(movement * Time.deltaTime);
        }
    }

    void CanDashApplySpeed(bool _state)
    {
        dashCanGainSpeed = _state;
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

    void AttackMove()
    {
        if (attackCanGainSpeed)
        {
            Vector3 movement = Vector3.zero;
            movement = transform.forward * attackMoveSpeed;
            controller.Move(movement * Time.deltaTime);
        }
    }

    public void CanMoveWhenAttacking(bool _state)
    {
        attackCanGainSpeed = _state;
    }

    public void KnockBack(Vector3 _direction, float _power, float _time)
    {
        if (playerStatus.invincible) return;

        StartCoroutine(KnockBackCoroutine(_direction, _power, _time));
    }

    IEnumerator KnockBackCoroutine(Vector3 _direction, float _power, float _time)
    {
        //controller.Move(_direction * _power * Time.deltaTime);

        //yield return new WaitForSeconds(_time);

        float wait = _time;

        Vector3 movement = Vector3.zero;
        movement = transform.forward * attackMoveSpeed;

        while (wait > 0)
        {
            wait -= Time.deltaTime;
            controller.Move(_direction * _power * Time.deltaTime);

            yield return null;
        }
        yield return null;
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
        //Gizmos.color = new Color(1, 0, 0, 0.5f);
        //Gizmos.DrawCube(boxPosition.position, boxSize);
    }
}
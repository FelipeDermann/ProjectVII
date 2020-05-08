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

    [Header("Attack Move Stop")]
    public BoxCollider attackMoveStopBoxPosition;
    public LayerMask attackMoveStopLayerMask;

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

    [Header("Particles")]
    public ParticleSystem[] dashParticles;
    public ParticleSystem hitParticle;

    //dashing
    Vector3 dashMoveDirection;
    float DashInputX;
    float DashInputZ;

    private void OnEnable()
    {
        DisableAttackState.FinishedAttack += CanWalkOn;
        AttackAnimationBehaviour.StartedAttack += CanWalkOff;

        DashBehaviour.DashStart += DashStart;
        DashBehaviour.DashEnd += DashEnd;
        
        PlayerAnimation.HurtAnimation += HurtParticle;

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

        PlayerAnimation.HurtAnimation -= HurtParticle;

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
        DashInput();
    }

    void HurtParticle()
    {
        hitParticle.Play();
    }

    void DashInput()
    {
        if (dashing) return;
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) return;
        DashInputX = Input.GetAxisRaw("Horizontal");
        DashInputZ = Input.GetAxisRaw("Vertical");
    }

    void Dash()
    {
        if (!Input.GetButtonDown("dash")) return;
        if (anim.GetBool("casting")) return;
        if (dashing) return;
        if (!canDash) return;

        //face input direction
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * DashInputZ + right * DashInputX;

        if (DashInputX != 0 || DashInputZ !=0) transform.rotation = Quaternion.LookRotation(desiredMoveDirection);

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

        for (int i = 0; i < dashParticles.Length; i++) dashParticles[i].Play();

        attack.DisableNextAttackInput();
    }

    public void DashEnd()
    {
        Debug.Log("DASH END ");

        anim.ResetTrigger("dash");
        if (!anim.GetBool("hurt")) CanWalkOn();

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
            var forward = cam.transform.forward;
            var right = cam.transform.right;
            desiredMoveDirection = forward * DashInputZ + right * DashInputX;

            //if ((DashInputZ == 0 && DashInputX == 0) && playerLockOn.isLocked) desiredMoveDirection = forward * transform.forward.z + right * transform.forward.x;
            if ((DashInputZ == 0 && DashInputX == 0) && !playerLockOn.isLocked) desiredMoveDirection = transform.forward;
            //if ((DashInputZ == 0 && DashInputX == 0)) desiredMoveDirection = transform.forward;

            controller.Move(desiredMoveDirection.normalized * Time.deltaTime * dashMoveSpeed);
        }
    }

    void CanDashApplySpeed(bool _state)
    {
        dashCanGainSpeed = _state;
    }

    public void CanWalkOn()
    {
        if (playerStatus.shopping) return;
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
            if (Physics.CheckBox(attackMoveStopBoxPosition.transform.position, attackMoveStopBoxPosition.bounds.size, Quaternion.identity, attackMoveStopLayerMask))
            {
                attackCanGainSpeed = false;
                Debug.Log("EBA");
            }

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
        if (playerStatus.ignoreStagger) return;

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

        verticalSpeed = -maxFallSpeed;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerStatus playerStatus;
    LockOn playerLockOn;
    Rigidbody rb;
    Attack attack;

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

    [Header("Basic Input")]
    public Vector2 input;
    public Vector3 velocity;
    private Vector3 inputDir;
    private Quaternion rotation;
    public float moveSpeed;
    public float rotationSpeed;
    public bool canMove;

    [Header("Attack Move Stop")]
    public BoxCollider attackMoveStopBoxPosition;
    public LayerMask attackMoveStopLayerMask;

    [Header("Ground Detection")]
    public bool isGrounded;
    public float fallingSpeedLimit;
    public Transform boxPosition;
    Vector3 boxSize;
    public LayerMask groundMask;
    public float slopeRaycastLength;
    public float slopeForce;

    [Header("Particles")]
    public ParticleSystem[] dashParticles;
    public ParticleSystem hitParticle;
    
    //dashing
    Vector3 dashMoveDirection;
    float DashInputX;
    float DashInputZ;

    private void Awake()
    {
        MagicBehaviour.UsingMagic += CastingMagic;

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
    private void OnDestroy()
    {
        MagicBehaviour.UsingMagic -= CastingMagic;

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
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        playerStatus = GetComponent<PlayerStatus>();
        playerLockOn = GetComponent<LockOn>();
        attack = GetComponent<Attack>();

        cam = Camera.main;

        boxSize = boxPosition.GetComponent<Collider>().bounds.size;

        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        DashMove();
        AttackMove();
        DetectGround();
        DashInput();

        Movement();
    }

    private void FixedUpdate()
    {
        ////gains speed
        velocity.y = rb.velocity.y;
        if(!isGrounded) DownForceWhenMidair();
        rb.velocity = velocity;
    }

    void CastingMagic(bool _canMove)
    {
        if (_canMove) CanWalkOff();
        else CanWalkOn();
    }

    private void ApplyOrientation()
    {
        if (attack.attacking) return;
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);
    }

    private void SetOrientation(Vector3 target)
    {
        rotation = Quaternion.LookRotation(target, Vector3.up);
        ApplyOrientation();
    }

    void Movement()
    {
        if (!canMove) return;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        if (input.magnitude > 0)
        {
            var camera = Camera.main;

            inputDir = camera.transform.forward * input.y;
            inputDir += camera.transform.right * input.x;
            inputDir.y = 0;
            inputDir.Normalize();
        }

        //turns to moving direction
        if (input.sqrMagnitude > 0.2f)
        {
            SetOrientation(inputDir);
        }
        else
        {
            inputDir = Vector3.zero;
        }

        //set speed variables
        velocity = inputDir * moveSpeed;

        ////////// ANIMATIONS
        anim.SetFloat("Blend", input.sqrMagnitude);
    }

    bool OnSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,1,0), Vector3.down, out hit, slopeRaycastLength, groundMask))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
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
        moveSpeed = 9;
        anim.SetBool("dashing", dashing);
    }

    void DashMove()
    {
        if (dashCanGainSpeed)
        {
            if (Physics.CheckBox(attackMoveStopBoxPosition.transform.position, attackMoveStopBoxPosition.bounds.size, Quaternion.identity, attackMoveStopLayerMask))
            {
                dashCanGainSpeed = false;
            }

            var forward = cam.transform.forward;
            var right = cam.transform.right;
            desiredMoveDirection = forward * DashInputZ + right * DashInputX;

            //if ((DashInputZ == 0 && DashInputX == 0) && playerLockOn.isLocked) desiredMoveDirection = forward * transform.forward.z + right * transform.forward.x;
            if ((DashInputZ == 0 && DashInputX == 0) && !playerLockOn.isLocked) desiredMoveDirection = transform.forward;
            //if ((DashInputZ == 0 && DashInputX == 0)) desiredMoveDirection = transform.forward;

            velocity = desiredMoveDirection.normalized * dashMoveSpeed;
        }
    }

    void CanDashApplySpeed(bool _state)
    {
        dashCanGainSpeed = _state;
    }

    public void CanWalkOn()
    {
        if (playerStatus.shopping) return;
        canMove = true;
    }

    public void CanWalkOff()
    {
        canMove = false;
        velocity.x = 0;
        velocity.z = 0;
    }

    void AttackMove()
    {
        if (attackCanGainSpeed)
        {
            if (Physics.CheckBox(attackMoveStopBoxPosition.transform.position, attackMoveStopBoxPosition.bounds.size, Quaternion.identity, attackMoveStopLayerMask))
            {
                attackCanGainSpeed = false;
            }

            Vector3 movement = Vector3.zero;
            movement = transform.forward * attackMoveSpeed;
            velocity = movement;
        }
    }

    public void CanMoveWhenAttacking(bool _state)
    {
        attackCanGainSpeed = _state;
        velocity.x = 0;
        velocity.z = 0;

    }

    public void KnockBack(Vector3 _direction, float _power, float _time)
    {
        if (playerStatus.invincible) return;
        if (playerStatus.ignoreStagger) return;
        // if (playerStatus.dead) return;

        rb.velocity = Vector3.zero;
        velocity = _direction * _power;

        //StartCoroutine(KnockBackCoroutine(_direction, _power, _time));
    }

    IEnumerator KnockBackCoroutine(Vector3 _direction, float _power, float _time)
    {
        float wait = _time;
        velocity = _direction * _power;
        yield return new WaitForSeconds(_time);
    }

    void DetectGround()
    {
        isGrounded = Physics.CheckBox(boxPosition.position, boxSize, Quaternion.identity, groundMask);
    }

    void DownForceWhenMidair()
    {
        float downForce = -1 * slopeForce;
        if (rb.velocity.y > fallingSpeedLimit) downForce = fallingSpeedLimit;

        velocity.y = downForce;
        Debug.Log(velocity);
    }

    public void ChangeDashState(bool _state)
    {
        canDash = _state;
    }
}
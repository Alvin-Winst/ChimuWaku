using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakuMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private Rigidbody2D rbChimu;
    private WakuSwim swimWaku;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    //[SerializeField] private PhysicsMaterial2D noFriction;
    //[SerializeField] private PhysicsMaterial2D fullFriction;

    private float xInput;
    private float yInput;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;

    private int facingDirection = 1;

    private bool isGrounded;
    private bool isOnSlope;
    private bool isJumping;
    private bool canWalkOnSlope;
    private bool canJump;

    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 colliderSize;
    private Vector2 slopeNormalPerp;


    private enum MovementState { idle, running, jumping, falling }

    //[SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        swimWaku = GameObject.Find("Swim").GetComponent<WakuSwim>();

        colliderSize = coll.size;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();

        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        CheckGround();
        SlopeCheck();
        ApplyMovement();
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal1");
        yInput = Input.GetAxisRaw("Vertical1");

        if (xInput == 1 && facingDirection == -1)
        {
            Flip();
        }
        else if (xInput == -1 && facingDirection == 1)
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump1"))
        {
            Jump();
        }

    }
    private void CheckGround()
    {
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, whatIsGround);

        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
        }

    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, colliderSize.y / 2));
        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);

    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;

        }

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
        {
            //rb.sharedMaterial = fullFriction;
        }
        else
        {
            //rb.sharedMaterial = noFriction;
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            isJumping = true;
            newVelocity.Set(0.0f, 0.0f);
            rb.velocity = newVelocity;
            newForce.Set(0.0f, jumpForce);
            rb.AddForce(newForce, ForceMode2D.Impulse);
        }
    }

    private void ApplyMovement()
    {
        Debug.Log(swimWaku.isInWater);
        if (isGrounded && !isOnSlope && !isJumping) //if not on slope
        {
            //Debug.Log("This one");
            newVelocity.Set(movementSpeed * xInput, 0.0f);
            rb.velocity = newVelocity;

            if (transform.Find("Platform").Find("Chimu"))
            {
                Vector2 chimuPos = transform.Find("Platform").Find("Chimu").position;
                rbChimu = transform.Find("Platform").Find("Chimu").GetComponent<Rigidbody2D>();
                Vector2 chimuVelocity = rbChimu.velocity;
                chimuPos.Set(chimuPos.x + newVelocity.x/75 + chimuVelocity.x/50, chimuPos.y + chimuVelocity.y/800);
                rbChimu.MovePosition(chimuPos);
            }
        }
        else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping && !swimWaku.isInWater) //If on slope
        {
            // Fix this rotation file
            if (!transform.Find("Platform").Find("Chimu"))
            {
                newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xInput, movementSpeed * slopeNormalPerp.y * -xInput);
                rb.velocity = newVelocity;
            }
            else
            {
                newVelocity.Set(0, 0);
                rb.velocity = newVelocity;
            }
        }
        else if (swimWaku.isInWater) //If in water
        {
            newVelocity.Set((movementSpeed - 3) * xInput, movementSpeed * yInput);
            rb.velocity = newVelocity;
        }

        else if (!isGrounded && !swimWaku.isInWater) //If in air
        {
            newVelocity.Set(movementSpeed * xInput, rb.velocity.y);
            rb.velocity = newVelocity;
        }

    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);

        if (transform.Find("Platform").Find("Chimu"))
        {
            transform.Find("Platform").Find("Chimu").Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (xInput > 0f)
        {
            state = MovementState.running;
        }
        else if (xInput < 0f)
        {
            state = MovementState.running;
        }
        else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) //If on slope
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .01f && !isGrounded)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.01f && !isGrounded)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

}
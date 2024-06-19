using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimuMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlatform;
    [SerializeField] private LayerMask whatIsMovingPlatform;
    //[SerializeField] private PhysicsMaterial2D noFriction;
    //[SerializeField] private PhysicsMaterial2D fullFriction;

    private float xInput;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;

    private int facingDirection = 1;

    private bool isGrounded;
    private bool isOnPlatform;
    private bool isOnMovingPlatform;
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
        xInput = Input.GetAxisRaw("Horizontal");

        if (xInput == 1 && facingDirection == -1)
        {
            Flip();
        }
        else if (xInput == -1 && facingDirection == 1)
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }
    private void CheckGround()
    {
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, whatIsGround);
        isOnPlatform = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, whatIsPlatform);
        isOnMovingPlatform = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, whatIsMovingPlatform);

        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if ((isGrounded || isOnPlatform || isOnMovingPlatform) && !isJumping && slopeDownAngle <= maxSlopeAngle)
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
        if ((isGrounded || isOnPlatform) && !isOnSlope && !isJumping) //if not on slope
        {
            //Debug.Log("This one");
            newVelocity.Set(movementSpeed * xInput, 0.0f);
            rb.velocity = newVelocity;
        }
        else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) //If on slope
        {
            // Fix this rotation file
            newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xInput, movementSpeed * slopeNormalPerp.y * -xInput);
            rb.velocity = newVelocity;
        }
        else if (!(isGrounded || isOnPlatform)) //If in air
        {
            newVelocity.Set(movementSpeed * xInput, rb.velocity.y);
            rb.velocity = newVelocity;
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
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

        if (rb.velocity.y > .01f && !(isGrounded || isOnPlatform))
        {
            state = MovementState.jumping;
            Debug.Log("Jumping");
        }
        else if (rb.velocity.y < -.01f && (isJumping || !isOnMovingPlatform))
        {
            state = MovementState.falling;
            Debug.Log("Free Falling");
        }
        else if (rb.velocity.y < -.01f && (isOnMovingPlatform))
        {
            state = MovementState.idle;
            Debug.Log("Falling Platform");
        }


        anim.SetInteger("state", (int)state);
    }

}
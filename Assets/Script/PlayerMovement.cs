using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float movSpeed = 7f;
    [SerializeField] private float jumpForce = 7.7f;

    [SerializeField] private float fallMultiplier = 2f;
    [SerializeField] private float lowJumpMultiplier = 1.5f;

    //[SerializeField] private int jumpMax = 1;  the commented code was for doubleJump function
    //private int jumpCount = 0;

    private enum MovementState { idle, running, jumping, falling }

    //[SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movSpeed, rb.velocity.y);


        //if (Input.GetButtonDown("Jump") && (IsGrounded() || jumpCount < jumpMax))  this code was for doubleJump function
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //  jumpCount++;
        }

        if (rb.velocity.y < -.01)
        {
            rb.velocity = rb.velocity + Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > .01 && !Input.GetButton("Jump"))
        {
            rb.velocity = rb.velocity + Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //if (IsGrounded())  This code was for doubleJump function
        //{
        //  jumpCount = 0;
        //}

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .01f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.01f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}

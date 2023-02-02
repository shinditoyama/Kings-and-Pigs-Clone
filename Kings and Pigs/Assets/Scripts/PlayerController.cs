using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    private float horizontal;
    private bool facingRight = true;
    private bool canMove = true;
    private bool isGrounded;
    private bool isJumping;

    // public GameObject objectToSpawn;

    [Header("Player")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        anim.SetFloat("SpeedX", Mathf.Abs(horizontal));
        anim.SetFloat("SpeedY", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        horizontal = ctx.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        isJumping = ctx.performed;
    }

    void FixedUpdate()
    {
        if (!canMove)
            return;

        Movement();
        CheckGround();
        Jump();
    }

    void Movement()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if ((horizontal < 0 && facingRight) || (horizontal > 0 && !facingRight))
        {
            Flip();
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.BoxCast(transform.position + (transform.right * -0.25f) + (transform.up * -0.45f), new Vector2(0.5f, 0.05f), 0f, Vector2.down, 0.01f, groundLayer);
    }

    void Jump()
    {
        if (isJumping && isGrounded)
        {
            isJumping = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (transform.right * -0.25f) + (transform.up * -0.45f), new Vector2(0.5f, 0.05f));
    }
}

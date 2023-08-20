using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float crouchSpeed = 2.5f;
    private float currentSpeed;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Jump")]
    public float jumpForce = 14f;
    public Transform groundCheck;
    private bool isGrounded;

    [Header("Crouch")]
    public Transform ceilingCheck;
    private bool isCeiling;
    private bool isCrouching;
    private Vector2 colliderOriginalSize;
    private Vector2 colliderOriginalOffset;
    public float crouchColliderScale = 0.5f;
    public Collider2D standingCollider;
    public Collider2D crouchingCollider;

    [Header("Air Control")]
    public float airControlFactor = 0.75f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colliderOriginalSize = standingCollider.bounds.size;
        colliderOriginalOffset = standingCollider.offset;
    }

    private void Update()
    {
        // Ground Check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Ceiling Check
        isCeiling = Physics2D.OverlapCircle(ceilingCheck.position, groundCheckRadius, whatIsGround);

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Crouch
        if (isGrounded && Input.GetButtonDown("Crouch"))
        {
            isCrouching = true;
            crouchingCollider.enabled = true;
            standingCollider.enabled = false;
            currentSpeed = crouchSpeed;
            transform.localScale = new Vector3(1f, crouchColliderScale, 1f);
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            // Check if there is enough space to stand up
            if (!isCeiling)
            {
                isCrouching = false;
                crouchingCollider.enabled = false;
                standingCollider.enabled = true;
                currentSpeed = moveSpeed;
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    private void FixedUpdate()
    {
        // Horizontal Movement
        float moveInput = Input.GetAxis("Horizontal");
        float targetVelocityX = moveInput * currentSpeed;

        if (!isGrounded)
        {
            // Apply air control
            targetVelocityX *= airControlFactor;
        }

        Vector2 targetVelocity = new Vector2(targetVelocityX, rb.velocity.y);
        rb.velocity = targetVelocity;
    }
}

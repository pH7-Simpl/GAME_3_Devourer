using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameEndMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int maxJumps;
    [SerializeField] private float doublePressTime;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public Animator animator;
    public float horizontal;
    public bool isFacingRight;
    private bool jumping;
    private int jumpsRemaining;
    private float lastPressTime;
    private bool canDash;
    private bool isDashing;
    private zoomOut zo;
    private void Awake()
    {
        zo = Camera.main.GetComponent<zoomOut>();
        speed = 8f;
        jumpPower = 8f;
        maxJumps = 1;
        doublePressTime = 0.2f;
        dashPower = 10f;
        dashingTime = 0.2f;
        dashingCooldown = 1f;
        jumpsRemaining = maxJumps;
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.transform.position = transform.position + new Vector3(0f, 0f, -5f);
        mainCamera.transform.SetParent(transform);
        isDashing = false;
        canDash = true;
        isFacingRight = true;
    }

    private void Update()
    {
        if (isDashing || zo.running)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        Flip();
        Jump();
        Dashing();
        animator.SetFloat("speed", Mathf.Abs(horizontal));
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        else
        {
            rb2D.velocity = new Vector2(horizontal * speed, rb2D.velocity.y);
        }
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.01f, groundLayer);
    }
    private void Flip()
    {
        bool shouldFlip = (isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f);
        if (shouldFlip)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            float yDirection = (jumpsRemaining == maxJumps) ? jumpPower : jumpPower * 1.1f;
            rb2D.velocity = new Vector2(rb2D.velocity.x, yDirection);
            jumpsRemaining--;
        }
        if (IsGrounded() && jumpsRemaining <= 0)
        {
            jumpsRemaining = maxJumps;
        }
        jumping = !IsGrounded();
        animator.SetBool("isJumping", jumping);
    }
    private void Dashing()
    {
        bool doublePressed = (Time.time - lastPressTime) < doublePressTime;

        if (canDash && doublePressed)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(RegularDash(1));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(RegularDash(-1));
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            lastPressTime = Time.time;
        }
    }
    private IEnumerator RegularDash(int direction)
    {
        canDash = false;
        isDashing = true;
        rb2D.gravityScale = 0f;
        rb2D.velocity = new Vector2(direction * dashPower, 0);
        yield return new WaitForSeconds(dashingTime);
        rb2D.gravityScale = 2f;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}

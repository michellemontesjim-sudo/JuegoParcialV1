using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;

    [Header("Wall & Raycast")]
    public Transform wallCheck; // place at player's side (center)
    public float wallCheckDistance = 0.2f;
    public float wallSlideSpeed = -1.5f;

    [Header("Settings")]
    public int extraLives = 0; // lives count given by pickups
    public Vector2 respawnOffset = Vector2.zero; // optional offset when respawning

    Rigidbody2D rb;
    Collider2D col;
    float horizontal;
    bool isGrounded;
    bool isTouchingWall;
    bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Recommend developer: set a PhysicsMaterial2D with zero friction to the collider to avoid sticking.
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        // wall detection (ray toward facing direction)
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;
        isTouchingWall = Physics2D.Raycast(wallCheck.position, dir, wallCheckDistance, groundMask);

        // Flip sprite if needed
        if (horizontal > 0.1f && !facingRight) Flip();
        else if (horizontal < -0.1f && facingRight) Flip();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Core horizontal movement: set velocity.x directly (prevents sticky forces)
        float targetVelX = horizontal * moveSpeed;

        // If touching wall and not grounded and moving toward the wall, allow controlled slide and prevent sticking:
        if (isTouchingWall && !isGrounded && Mathf.Abs(horizontal) > 0.01f)
        {
            // allow horizontal movement but don't let us "glue" to wall: reduce x a bit
            targetVelX = horizontal * moveSpeed * 0.9f;
            // allow a small sliding down when falling
            if (rb.velocity.y < 0f)
                rb.velocity = new Vector2(targetVelX, Mathf.Max(rb.velocity.y, wallSlideSpeed));
            else
                rb.velocity = new Vector2(targetVelX, rb.velocity.y);
        }
        else
        {
            // normal movement
            rb.velocity = new Vector2(targetVelX, rb.velocity.y);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
    }

    // helpers
    public Vector3 GetPosition() => transform.position;

    // Add life method called by pickups
    public void AddLife()
    {
        extraLives += 1;
        // optionally heal a bit (not required)
        Debug.Log("Extra life picked. Total extra lives: " + extraLives);
    }

    // A respawn helper called by SceneTransitionManager or DeathZone if you want to respawn instead of going to gameover
    public void RespawnAt(Vector3 pos)
    {
        rb.velocity = Vector2.zero;
        transform.position = pos + (Vector3)respawnOffset;
    }
}

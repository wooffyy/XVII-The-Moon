using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    [Header("Gravity Flip")]
    public bool onlyFlipWhenGrounded = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    float moveInput;
    bool running;
    bool flipPressed;
    bool isGrounded;

    float baseGravity;
    float gravityDir = 1f;   // 1 = normal, -1 = flipped

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        baseGravity = rb.gravityScale;
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        moveInput = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  moveInput -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveInput += 1f;

        running = kb.leftShiftKey.isPressed;

        if (kb.spaceKey.wasPressedThisFrame) flipPressed = true;

        // Face movement direction
        if (moveInput != 0f) sr.flipX = moveInput < 0f;

        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        float speed = running ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (flipPressed && (isGrounded || !onlyFlipWhenGrounded))
            FlipGravity();

        flipPressed = false;
    }

    void FlipGravity()
    {
        gravityDir *= -1f;
        rb.gravityScale = baseGravity * gravityDir;

        // Mirror the whole player vertically (sprite, collider and GroundCheck together)
        Vector3 s = transform.localScale;
        s.y = Mathf.Abs(s.y) * gravityDir;
        transform.localScale = s;

        // Clear leftover vertical speed so the flip feels snappy
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
using UnityEngine;

public class Player01Movement : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 7f;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    bool isGrounded;
    Collider2D playerCollider;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Move
        float move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Jump
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump); // rb.linearVelocity.x helps keep the x movement so that when player jumps it doesnt happen in one place
        }
        if (Input.GetKey(KeyCode.S))
        {
            Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            if (platform != null)
            {
                StartCoroutine(DisablePlatform(platform));
            }
        }
        System.Collections.IEnumerator DisablePlatform(Collider2D platform)
        {
            // Ignore only the platform you are standing on
            Physics2D.IgnoreCollision(playerCollider, platform, true);
            yield return new WaitForSeconds(0.3f);
            Physics2D.IgnoreCollision(playerCollider, platform, false);
        }
    }
}

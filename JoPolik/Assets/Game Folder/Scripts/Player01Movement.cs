//using UnityEngine;
//using System.Collections; // Needed for IEnumerator

//public class Player01Movement : MonoBehaviour
//{
//    public float speed = 5f;
//    public float jump = 7f;
//    public Transform groundCheck;
//    public float checkRadius = 0.2f;
//    public LayerMask groundLayer;

//    Rigidbody2D rb;
//    bool isGrounded;
//    Collider2D playerCollider;
//    Animator animator; // Added Animator

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        playerCollider = GetComponent<Collider2D>();
//        animator = GetComponent<Animator>(); // Get Animator
//    }

//    void Update()
//    {
//        // Move
//        float move = Input.GetAxisRaw("Horizontal");
//        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

//        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

//        // Set animator parameters (Added)
//        animator.SetFloat("Float Speed", Mathf.Abs(move)); // Run animation when pressing A/D
//        // Flip sprite
//        if (move > 0) transform.localScale = new Vector3(1, 1, 1);
//        else if (move < 0) transform.localScale = new Vector3(-1, 1, 1);


//        // Jump
//        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
//        {
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
//            animator.SetTrigger("Trigger Jump"); // Added Jump trigger
//        }

//        // Drop through platform
//        if (Input.GetKey(KeyCode.S))
//        {
//            Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
//            if (platform != null)
//            {
//                StartCoroutine(DisablePlatform(platform));
//            }
//        }
//    }

//    IEnumerator DisablePlatform(Collider2D platform)
//    {
//        Physics2D.IgnoreCollision(playerCollider, platform, true);
//        yield return new WaitForSeconds(0.3f);
//        Physics2D.IgnoreCollision(playerCollider, platform, false);
//    }
//}

using UnityEngine;
using System.Collections; // Needed for IEnumerator

public class Player01Movement : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 7f;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    private Vector3 originalScale;

    Rigidbody2D rb;
    bool isGrounded;
    Collider2D playerCollider;
    Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Move
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Update Blend Tree parameter
        animator.SetFloat("Float Speed", Mathf.Abs(moveInput));

        // Flip sprite
        if (moveInput > 0)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // Jump
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            animator.SetTrigger("Trigger Jump");
        }

        // Drop through platform
        if (Input.GetKey(KeyCode.S))
        {
            Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            if (platform != null)
            {
                StartCoroutine(DisablePlatform(platform));
            }
        }
    }

    IEnumerator DisablePlatform(Collider2D platform)
    {
        Physics2D.IgnoreCollision(playerCollider, platform, true);
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreCollision(playerCollider, platform, false);
    }
}

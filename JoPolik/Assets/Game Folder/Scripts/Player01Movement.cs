using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Player01Movement : MonoBehaviourPun, IPunObservable
{
    public float speed = 5f;
    public float jump = 7f;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    private Vector3 originalScale;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Collider2D playerCollider;
    private Animator animator;

    // network smoothing
    private Vector3 networkPosition;
    private Vector2 networkVelocity;
    private float networkScaleX;
    private float networkAnimSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        networkPosition = transform.position;
        networkScaleX = transform.localScale.x;
    }

    void Start()
    {
        // If this is not the local player, disable physics simulation to avoid conflicts
        if (!photonView.IsMine)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            // disable local-only components, e.g., camera
            Camera c = GetComponentInChildren<Camera>();
            if (c) c.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Local player controls
        if (photonView.IsMine)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

            // Update Blend Tree parameter (use "Speed" in your Animator/Blend Tree)
            animator.SetFloat("Float Speed", Mathf.Abs(moveInput));

            // Flip sprite without changing original scale magnitude
            if (moveInput > 0)
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            else if (moveInput < 0)
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

            // Jump
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            }

            // Drop through platform
            if (Input.GetKey(KeyCode.S))
            {
                Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
                if (platform != null)
                    StartCoroutine(DisablePlatform(platform));
            }
        }
        else
        {
            // Remote player interpolation
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 12f);
            // apply received flip
            Vector3 s = transform.localScale;
            s.x = networkScaleX;
            transform.localScale = s;
            // animator for remote players: set speed param received
            animator.SetFloat("Float Speed", networkAnimSpeed);
        }
    }

    IEnumerator DisablePlatform(Collider2D platform)
    {
        Physics2D.IgnoreCollision(playerCollider, platform, true);
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreCollision(playerCollider, platform, false);
    }

    // IPunObservable: send/receive minimal data over network
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // local player -> send to others
        {
            stream.SendNext(transform.position);
            stream.SendNext(rb.linearVelocity);
            stream.SendNext(transform.localScale.x);
            stream.SendNext(animator.GetFloat("Float Speed"));
        }
        else // remote player -> receive
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkVelocity = (Vector2)stream.ReceiveNext();
            networkScaleX = (float)stream.ReceiveNext();
            networkAnimSpeed = (float)stream.ReceiveNext();
        }
    }
}

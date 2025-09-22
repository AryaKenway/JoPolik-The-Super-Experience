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

    private Vector3 networkPosition;
    private Vector2 networkVelocity;
    private float networkScaleX;
    private float networkAnimSpeed;

    public Vector3 respawnPoint = new Vector3(0, 2, 0);
    public float deathYThreshold = -3f;
    private bool isDead = false;

    public bool canMove = true;

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
        if (!photonView.IsMine)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            Camera c = GetComponentInChildren<Camera>();
            if (c) c.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (photonView.IsMine && !isDead)
        {
            if (!QuestionManager2D.IsQuestionActive && canMove)
            {
                float moveInput = Input.GetAxisRaw("Horizontal");
                rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

                isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

                animator.SetFloat("Float Speed", Mathf.Abs(moveInput));

                if (moveInput > 0)
                    transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                else if (moveInput < 0)
                    transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

                if (Input.GetKeyDown(KeyCode.W) && isGrounded)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
                }

                if (Input.GetKey(KeyCode.S))
                {
                    Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
                    if (platform != null)
                        StartCoroutine(DisablePlatform(platform));
                }
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                animator.SetFloat("Float Speed", 0f);
            }

            if (transform.position.y < deathYThreshold)
            {
                StartCoroutine(DieAndRespawn());
            }
        }
        else if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 12f);
            Vector3 s = transform.localScale;
            s.x = networkScaleX;
            transform.localScale = s;
            animator.SetFloat("Float Speed", networkAnimSpeed);
        }
    }

    IEnumerator DisablePlatform(Collider2D platform)
    {
        Physics2D.IgnoreCollision(playerCollider, platform, true);
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreCollision(playerCollider, platform, false);
    }

    IEnumerator DieAndRespawn()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        playerCollider.enabled = false;

        yield return new WaitForSeconds(2f);

        transform.position = respawnPoint;
        rb.simulated = true;
        playerCollider.enabled = true;
        isDead = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(rb.linearVelocity);
            stream.SendNext(transform.localScale.x);
            stream.SendNext(animator.GetFloat("Float Speed"));
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkVelocity = (Vector2)stream.ReceiveNext();
            networkScaleX = (float)stream.ReceiveNext();
            networkAnimSpeed = (float)stream.ReceiveNext();
        }
    }
}

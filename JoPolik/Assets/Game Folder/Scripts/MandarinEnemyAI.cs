using UnityEngine;
using Photon.Pun;

public class MandarinEnemyAI : MonoBehaviourPun, IPunObservable
{
    public Transform player;
    public Transform groundCheck;
    public float speed = 3f;
    public float followDuration = 5f;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;

    private bool isFollowing = false;
    private float followTimer = 0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isFollowing)
            {
                followTimer -= Time.deltaTime;

                if (followTimer > 0)
                {
                    FollowPlayer();
                }
                else
                {
                    isFollowing = false;
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                }
            }
        }
    }

    public void TriggerFollow()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isFollowing = true;
            followTimer = followDuration;
        }
    }

    private void FollowPlayer()
    {
        bool groundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (!groundAhead)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (player != null)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

            if (direction > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (direction < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(rb.linearVelocity);
            stream.SendNext(isFollowing);
            stream.SendNext(followTimer);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            rb.linearVelocity = (Vector2)stream.ReceiveNext();
            isFollowing = (bool)stream.ReceiveNext();
            followTimer = (float)stream.ReceiveNext();
        }
    }
}

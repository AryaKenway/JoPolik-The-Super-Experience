using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    public Transform player;          // reference to the player
    private float spriteWidth;        // width of the background image
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Follow the player's movement
        float newPosition = Mathf.Repeat(player.position.x, spriteWidth);
        transform.position = startPosition + Vector3.right * newPosition;
    }
}

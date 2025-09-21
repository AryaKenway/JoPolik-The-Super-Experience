using UnityEngine;

public class TotemInteraction : MonoBehaviour
{
    public string questionID = "q1";          // ID of the question for this NPC
    public QuestionManager2D questionManager; // Drag your QuestionManager2D in Inspector

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacting with NPC...");
            questionManager.ShowQuestion(questionID);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player is near NPC");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player left NPC area");
        }
    }
}

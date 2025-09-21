using UnityEngine;

public class ThugNPCInteraction : MonoBehaviour
{
    public QuestionManager questionManager; // reference to QuestionManager
    public string questionID; // unique ID of the question this NPC asks

    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            // Open question UI
            questionManager.ShowQuestion(questionID);
        }
    }
}

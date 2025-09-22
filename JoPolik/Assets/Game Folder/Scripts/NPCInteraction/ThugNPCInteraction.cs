using UnityEngine;

public class ThugNPCInteraction : MonoBehaviour
{
    public QuestionManager questionManager; 
    public string questionID; 

    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            questionManager.ShowQuestion(questionID);
        }
    }
}

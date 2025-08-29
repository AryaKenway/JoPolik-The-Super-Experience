using UnityEngine;
using TMPro;

public class BobInteraction : MonoBehaviour
{
    private bool isPlayerNearby = false;
    public TextMeshPro dialogueText;  // 3D TextMeshPro (world object)

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detect only local player (Photon example)
        var view = other.gameObject.GetComponentInParent<Photon.Pun.PhotonView>();
        if (other.CompareTag("Player") && view != null && view.IsMine)
        {
            isPlayerNearby = true;
            dialogueText.gameObject.SetActive(true);
            dialogueText.text = "E";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var view = other.gameObject.GetComponentInParent<Photon.Pun.PhotonView>();
        if (other.CompareTag("Player") && view != null && view.IsMine)
        {
            isPlayerNearby = false;
            dialogueText.gameObject.SetActive(false);
        }
    }

    private void ToggleDialogue()
    {
        if (dialogueText.text == "Hello")
        {
            dialogueText.text = "E";
        }
        else
        {
            dialogueText.text = "Hello";
        }
    }
}

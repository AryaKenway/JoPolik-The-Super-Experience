using UnityEngine;
using Photon.Pun; // needed for PhotonNetwork.LoadLevel

public class ScenePortal : MonoBehaviour
{
    public string sceneName = "Scene_01"; // name of your 3D scene

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }
        
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}

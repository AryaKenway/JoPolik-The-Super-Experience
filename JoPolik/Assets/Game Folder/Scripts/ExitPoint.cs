using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitPoint : MonoBehaviour
{
    public float requiredHold = 0f; 
    public string displayText = "Press E to Exit Game";
    public GameObject visual;

    public void SetupDisplay(string text)
    {
        displayText = text;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Exit triggered by player at " + transform.position);
            QuitGame();
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Stopping play mode (Editor).");
        EditorApplication.isPlaying = false;
#else
        Debug.Log("Quitting application.");
        Application.Quit();
#endif
    }
}

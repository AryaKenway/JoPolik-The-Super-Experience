using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads the next scene (make sure to add it in Build Settings)
    public void PlayGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Quits the application (works only in build, not editor)
    public void QuitGame()
    {
        Debug.Log("Quit pressed!");
        Application.Quit();
    }
}

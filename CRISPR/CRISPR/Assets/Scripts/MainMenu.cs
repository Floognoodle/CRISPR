using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called by the Play button
    public void Play()
    {
        // Load the first level
        SceneManager.LoadSceneAsync("Level1");
    }

    // Called by the Quit button
    public void Quit()
    {
        Application.Quit();
    }
}
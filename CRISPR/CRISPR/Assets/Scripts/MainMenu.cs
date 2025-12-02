using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
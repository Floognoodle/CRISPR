using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletedUI : MonoBehaviour
{
    public static LevelCompletedUI Instance { get; private set; }

    // Shows "Level Completed" UI
    public GameObject levelCompletedPanel;

    // Name of the next level
    public string nextSceneName = "";

    // Pause when shown
    public bool pauseOnShow = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Hide at start
        if (levelCompletedPanel != null)
            levelCompletedPanel.SetActive(false);
    }

    // Show the level completed menu
    public void Show()
    {
        if (levelCompletedPanel == null) return;

        levelCompletedPanel.SetActive(true);

        if (pauseOnShow)
            Time.timeScale = 0f;
    }

    public void Hide()
    {
        if (levelCompletedPanel == null) return;

        levelCompletedPanel.SetActive(false);

        if (pauseOnShow)
            Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        if (pauseOnShow)
            Time.timeScale = 1f;

        // Use level names when possible
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // Otherwise
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene(currentIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}